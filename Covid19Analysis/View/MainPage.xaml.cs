using System;
using System.Collections.Generic;
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
            this.InitializeComponent();
            this.DataCreator = new CovidDataCreator();
            this.LoadedDataCollection = new CovidDataCollection();
            this.UpperBoundaryLimit = GetBoundariesContentDialog.UpperBoundaryDefault;
            this.LowerBoundaryLimit = GetBoundariesContentDialog.LowerBoundaryDefault;
            this.BinSize = BinChangerContentDialog.DefaultBinSize;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        private async void ChangeBoundaries_Click(object sender, RoutedEventArgs e)
        {
            var boundaryContentDialog = new GetBoundariesContentDialog();

            var result = await boundaryContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.UpperBoundaryLimit = boundaryContentDialog.UpperBoundary;
                this.LowerBoundaryLimit = boundaryContentDialog.LowerBoundary;
                if (this.LoadedDataCollection.Count > 0)
                {
                    this.createNewReportSummary();
                }
            }
        }

        private async void ChangeBinSize_Click(object sender, RoutedEventArgs e)
        {
            var binChangerContentDialog = new BinChangerContentDialog();

            var result = await binChangerContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.BinSize = binChangerContentDialog.BinSize;
                if (this.LoadedDataCollection.Count > 0)
                {
                    this.createNewReportSummary();
                }
            }
        }

        private async void AddData_Click(object sender, RoutedEventArgs e)
        {
            var addDataContentDialog = new AddCovidDataContentDialog();

            var result = await addDataContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var data = new CovidData(addDataContentDialog.DateDate,
                    addDataContentDialog.State, addDataContentDialog.PositiveCaseIncrease,
                    addDataContentDialog.NegativeCaseIncrease,
                    addDataContentDialog.CurrHospitalized,
                    addDataContentDialog.DeathNumbers,
                    addDataContentDialog.HospitalizedNumbers);
                if (this.LoadedDataCollection.Any(covidData => covidData.Date == data.Date))
                {
                    await this.handleDuplicateDay(data);
                }
                else
                {
                    this.LoadedDataCollection.Add(data);
                }

                this.createNewReportSummary();
            }
        }

        private async void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("CSV", new List<string> {".csv"});
            savePicker.SuggestedFileName = "New Document";

            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                var text = FileHeader + Environment.NewLine;
                foreach (var currData in this.LoadedDataCollection)
                {
                    text += currData + Environment.NewLine;
                }

                await FileIO.WriteTextAsync(file, text);
            }
        }

        private async void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail, SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                await this.processFile(file);
            }
            else
            {
                this.summaryTextBox.Text = "Operation cancelled.";
            }
        }

        private async Task processFile(StorageFile file)
        {
            var lines = await getFileLines(file);
            this.DataCreator.CreateCovidData(lines);
            var state = DefaultStateSelector;
            if (this.stateComboBox.SelectedValue != null)
            {
                state = this.stateComboBox.SelectedValue.ToString();
            }
            var stateCovidData = this.DataCreator.GetStateCovidData(state);
            if (this.LoadedDataCollection.Count > 0)
            {
                this.handleExistingFileLoading(stateCovidData);
            }
            else
            {
                this.LoadedDataCollection = stateCovidData;
            }
            this.createNewReportSummary();
        }

        private async void handleExistingFileLoading(CovidDataCollection covidCollection)
        {
            var loadingDialog = new ContentDialog {
                Title = "There Is Already A File Loaded",
                Content = "Do You Wish To Replace Or Merge This File?",
                PrimaryButtonText = "Replace",
                SecondaryButtonText = "Merge"
            };

            var result = await loadingDialog.ShowAsync();

            if (result == Replace)
            {
                this.LoadedDataCollection = covidCollection;
                this.createNewReportSummary();
            }

            if (result == Merge)
            {
                this.mergeFile(covidCollection);
            }
        }

        private async void mergeFile(CovidDataCollection covidCollection)
        {
            foreach (var currCovidData in covidCollection)
            {
                if (this.LoadedDataCollection.Any(covidData => covidData.Date == currCovidData.Date))
                {
                    await this.handleDuplicateDay(currCovidData);
                }
                else
                {
                    this.LoadedDataCollection.Add(currCovidData);
                }
            }

            this.createNewReportSummary();
        }

        private async Task handleDuplicateDay(CovidData currCovidData)
        {
            var result = await showDuplicateDayDialog();

            if (result == Replace)
            {
                this.replaceDuplicateDay(currCovidData);
            }
            else if (result == Merge)
            {
                this.mergeDuplicateDay(currCovidData);
            }
        }

        private void replaceDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = this.LoadedDataCollection.First(covidData =>
                covidData.Date == duplicateDay);
            var index = this.LoadedDataCollection.IndexOf(day);
            this.LoadedDataCollection[index] = currCovidData;
        }

        private void mergeDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = this.LoadedDataCollection.First(covidData => covidData.Date == duplicateDay);
            var index = this.LoadedDataCollection.IndexOf(day);
            day.PositiveCasesIncrease += currCovidData.PositiveCasesIncrease;
            day.NegativeCasesIncrease += currCovidData.NegativeCasesIncrease;
            day.DeathNumbers += currCovidData.DeathNumbers;
            day.HospitalizedNumbers += currCovidData.HospitalizedNumbers;
            this.LoadedDataCollection[index] = day;
        }

        private static IAsyncOperation<ContentDialogResult> showDuplicateDayDialog()
        {
            var duplicateDayDialog = new ContentDialog {
                Title = "Duplicate Day Found",
                Content = "Do you want to replace day data or merge?",
                PrimaryButtonText = "Replace",
                SecondaryButtonText = "Merge"
            };

            var result = duplicateDayDialog.ShowAsync();
            return result;
        }

        private void createNewReportSummary()
        {
            var stateMonthData = new MonthlyCovidDataCollection(this.LoadedDataCollection);
            var covidFormatter = new CovidDataFormatter(this.LoadedDataCollection);
            this.summaryTextBox.Text = "";
            this.summaryTextBox.Text =
                covidFormatter.FormatGeneralData(this.UpperBoundaryLimit, this.LowerBoundaryLimit, this.BinSize);
            this.summaryTextBox.Text += covidFormatter.FormatMonthlyData(stateMonthData);
        }

        private async void ErrorLines_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataCreator.ErrorLines.Count == 0)
            {
                var errorDialog = new ContentDialog {
                    Title = "Lines With Errors",
                    Content = "No Lines With Errors",
                    PrimaryButtonText = "Close"
                };

                await errorDialog.ShowAsync();
            }
            else
            {
                var errorDialog = new ContentDialog {
                    Title = "Lines With Errors",
                    Content = new CovidDataFormatter(this.LoadedDataCollection).ErrorLinesToString(this.DataCreator),
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
        public const int ApplicationHeight = 700;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 1000;

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

        /// <summary>
        ///     The bin size to be used for histogram
        /// </summary>
        public int BinSize { get; set; }

        private const ContentDialogResult Replace = ContentDialogResult.Primary;

        private const ContentDialogResult Merge = ContentDialogResult.Secondary;

        private const string FileHeader = "date, state, positiveCases, negativeCases, death, hospitalized";

        #endregion

        private void stateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = this.stateComboBox.SelectedValue;
            if (selectedValue != null )
            {
                if (this.DataCreator.CovidData.Count > 0)
                {
                    var stateData = this.DataCreator.GetStateCovidData(selectedValue.ToString());
                    this.LoadedDataCollection = stateData;
                    this.createNewReportSummary();
                }
            }
        }
    }
}