using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.DataHandling;
using Covid19Analysis.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Covid19Analysis.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DataCreator = new CovidDataCreator();
            LoadedDataCollection = new CovidDataCollection();
            UpperBoundaryLimit = GetBoundariesContentDialog.UpperBoundaryDefault;
            LowerBoundaryLimit = GetBoundariesContentDialog.LowerBoundaryDefault;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        private async void changeBoundaries_Click(object sender, RoutedEventArgs e)
        {
            var boundaryContentDialog = new GetBoundariesContentDialog();

            var result = await boundaryContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                UpperBoundaryLimit = boundaryContentDialog.UpperBoundary;
                LowerBoundaryLimit = boundaryContentDialog.LowerBoundary;
                if (LoadedDataCollection.Count > 0) CreateNewReportSummary();
            }
        }

        private async void addData_Click(object sender, RoutedEventArgs e)
        {
            var addDataContentDialog = new AddCovidDataContentDialog();

            var result = addDataContentDialog.ShowAsync();

            if (result.GetResults() == ContentDialogResult.Primary)
            {
                CovidData data = new CovidData(addDataContentDialog.DateDate, 
                    addDataContentDialog.State, addDataContentDialog.PositiveCaseIncrease, 
                    addDataContentDialog.NegativeCaseIncrease, 
                    addDataContentDialog.DeathNumbers, 
                    addDataContentDialog.HospitalizedNumbers);

                if (this.LoadedDataCollection.CovidRecords.Any(covidData => covidData.Date == data.Date))
                {
                    await this.handleDuplicateDay(data);
                }
                else
                {
                    this.LoadedDataCollection.Add(data);
                }

                CreateNewReportSummary();
            }
        }

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail, SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
                await processFile(file);
            else
                SummaryTextBox.Text = "Operation cancelled.";
        }

        private async Task processFile(StorageFile file)
        {
            var lines = await getFileLines(file);
            DataCreator.CreateCovidData(lines);
            var stateCovidData = DataCreator.GetStateCovidData(DefaultStateSelector);
            if (LoadedDataCollection.Count > 0)
                handleExistingFileLoading(stateCovidData);
            else
                LoadedDataCollection = stateCovidData;
            CreateNewReportSummary();
        }

        private void clearData_Click(object sender, RoutedEventArgs e)
        {
            LoadedDataCollection.CovidRecords.Clear();
            SummaryTextBox.Text = "";
        }

        private async void handleExistingFileLoading(CovidDataCollection covidCollection)
        {
            var loadingDialog = new ContentDialog
            {
                Title = "There Is Already A File Loaded",
                Content = "Do You Wish To Replace Or Merge This File?",
                PrimaryButtonText = "Replace",
                SecondaryButtonText = "Merge"
            };

            var result = await loadingDialog.ShowAsync();

            if (result == Replace)
            {
                LoadedDataCollection = covidCollection;
                CreateNewReportSummary();
            }

            if (result == Merge) mergeFile(covidCollection);
        }

        private async void mergeFile(CovidDataCollection covidCollection)
        {
            foreach (var currCovidData in covidCollection.CovidRecords)
                if (LoadedDataCollection.CovidRecords.Any(covidData => covidData.Date == currCovidData.Date))
                {
                    await handleDuplicateDay(currCovidData);
                }
                else
                {
                    LoadedDataCollection.Add(currCovidData);
                }

            CreateNewReportSummary();
        }

        private async Task handleDuplicateDay(CovidData currCovidData)
        {
            var result = await showDuplicateDayDialog();

            if (result == Replace)
                replaceDuplicateDay(currCovidData);
            else if (result == Merge) mergeDuplicateDay(currCovidData);
        }

        private void replaceDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = LoadedDataCollection.CovidRecords.First(covidData =>
                covidData.Date == duplicateDay);
            var index = LoadedDataCollection.CovidRecords.IndexOf(day);
            LoadedDataCollection.CovidRecords[index] = currCovidData;
        }

        private void mergeDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = LoadedDataCollection.CovidRecords.First(covidData => covidData.Date == duplicateDay);
            var index = LoadedDataCollection.CovidRecords.IndexOf(day);
            day.PositiveCasesIncrease += currCovidData.PositiveCasesIncrease;
            day.NegativeCasesIncrease += currCovidData.NegativeCasesIncrease;
            day.DeathNumbers += currCovidData.DeathNumbers;
            day.HospitalizedNumbers += currCovidData.HospitalizedNumbers;
            LoadedDataCollection.CovidRecords[index] = day;
        }

        private static IAsyncOperation<ContentDialogResult> showDuplicateDayDialog()
        {
            var duplicateDayDialog = new ContentDialog
            {
                Title = "Duplicate Day Found",
                Content = "Do you want to replace day data or merge?",
                PrimaryButtonText = "Replace",
                SecondaryButtonText = "Merge"
            };

            var result = duplicateDayDialog.ShowAsync();
            return result;
        }

        private void CreateNewReportSummary()
        {
            var stateMonthData = new MonthlyCovidDataCollection(LoadedDataCollection);
            var covidFormatter = new CovidDataFormatter(LoadedDataCollection);
            SummaryTextBox.Text = "";
            SummaryTextBox.Text = covidFormatter.FormatGeneralData(UpperBoundaryLimit, LowerBoundaryLimit);
            SummaryTextBox.Text += covidFormatter.FormatMonthlyData(stateMonthData);
        }

        private async void errorLines_Click(object sender, RoutedEventArgs e)
        {
            if (DataCreator.ErrorLines.Count == 0)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lines With Errors",
                    Content = "No Lines With Errors",
                    PrimaryButtonText = "Close"
                };

                await errorDialog.ShowAsync();
            }
            else
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lines With Errors",
                    Content = new CovidDataFormatter(LoadedDataCollection).ErrorLinesToString(DataCreator),
                    PrimaryButtonText = "Close"
                };

                await errorDialog.ShowAsync();
            }
        }

        private static async Task<string[]> getFileLines(StorageFile file)
        {
            var fileText = await FileIO.ReadTextAsync(file);
            var lines = fileText.Split("\r\n");
            return lines;
        }

        #endregion

        #region CovidRecords members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 355;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

        /// <summary>
        ///     The default State selector to get from a covid collection
        /// </summary>
        public const string DefaultStateSelector = "GA";

        /// <summary>
        ///     The upper boundary limit of the threshold
        /// </summary>
        public int UpperBoundaryLimit { get; set; }

        /// <summary>
        ///     the lower boundary limit of the threshold
        /// </summary>
        public int LowerBoundaryLimit { get; set; }

        /// <summary>
        ///     The CovidData Collection that is loaded into the app
        /// </summary>
        public CovidDataCollection LoadedDataCollection { get; set; }

        /// <summary>
        ///     The DataCreator for the application
        /// </summary>
        public CovidDataCreator DataCreator { get; set; }

        private const ContentDialogResult Replace = ContentDialogResult.Primary;

        private const ContentDialogResult Merge = ContentDialogResult.Secondary;

        #endregion
    }
}