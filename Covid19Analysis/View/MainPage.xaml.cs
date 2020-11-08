using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.DataHandling;
using Covid19Analysis.Model;
using Covid19Analysis.ViewModel;


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
            DataController = new CovidDataController();
            DataCreator = new CovidDataCreator();
            LoadedDataCollection = new CovidDataCollection();
            UpperBoundaryLimit = GetBoundariesContentDialog.UpperBoundaryDefault;
            LowerBoundaryLimit = GetBoundariesContentDialog.LowerBoundaryDefault;
            BinSize = BinChangerContentDialog.DefaultBinSize;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            DataController = (CovidDataController) DataContext;
        }

        #endregion

        private void stateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = StateComboBox.SelectedValue;
            if (selectedValue != null)
                if (DataCreator.CovidData.Count > 0)
                {
                    var stateData = DataCreator.GetStateCovidData(selectedValue.ToString());
                    LoadedDataCollection = stateData;
                    CreateNewReportSummary();
                }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            LoadedDataCollection.Remove(DataController.SelectedCovidData);
            CreateNewReportSummary();
        }

        private void UpdateButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DataController.SelectedCovidData.Date;
            var state = DataController.SelectedCovidData.State;
            var positiveCases = int.Parse(PositiveCasesTextBox.Text);
            var negativeCases = int.Parse(NegativeCasesTextBox.Text);
            var currentHospitalized = int.Parse(CurrentHospitalizedTextBox.Text);
            var deaths = int.Parse(DeathsTextBox.Text);
            var hospitalized = int.Parse(HospitalizedTextBox.Text);

            var covidData = new CovidData(date, state, positiveCases, negativeCases, currentHospitalized, deaths,
                hospitalized);
            DataController.SelectedCovidData = covidData;
            LoadedDataCollection.ReplaceCovidData(covidData);

            DataController.HandleSelectionUpdate();
            CreateNewReportSummary();
        }

        #region Methods

        private async void ChangeBoundaries_Click(object sender, RoutedEventArgs e)
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

        private async void ChangeBinSize_Click(object sender, RoutedEventArgs e)
        {
            var binChangerContentDialog = new BinChangerContentDialog();

            var result = await binChangerContentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                BinSize = binChangerContentDialog.BinSize;
                if (LoadedDataCollection.Count > 0) CreateNewReportSummary();
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
                if (LoadedDataCollection.Any(covidData => covidData.Date == data.Date))
                    await HandleDuplicateDay(data);
                else
                    LoadedDataCollection.Add(data);

                DataController.SetObservableCollection(LoadedDataCollection);
                CreateNewReportSummary();
            }
        }

        private async void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("CSV", new List<string> {".csv"});
            savePicker.FileTypeChoices.Add("XML", new List<string> {".xml"});
            savePicker.SuggestedFileName = "New Document";

            var file = await savePicker.PickSaveFileAsync();
            if (file != null && file.FileType.Equals(".xml"))
            {
                var serializer = new XmlSerializer(typeof(CovidDataCollection));
                var outStream = await file.OpenStreamForWriteAsync();
                serializer.Serialize(outStream, LoadedDataCollection);
                outStream.Dispose();
            }
            else if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                var text = FileHeader + Environment.NewLine;
                foreach (var currData in LoadedDataCollection) text += currData + Environment.NewLine;

                await FileIO.WriteTextAsync(file, text);
            }
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
        {
            LoadedDataCollection.Clear();
            SummaryTextBox.Text = "";
            DataController.SetObservableCollection(LoadedDataCollection);
        }

        private async void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail, SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");
            openPicker.FileTypeFilter.Add(".xml");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null && file.FileType.Equals(".xml"))
            {
                await DataCreator.DeserializeCovidData(file);
                //await
                ProcessData();
            }
            else if (file != null)
            {
                var lines = await GetFileLines(file);
                DataCreator.CreateCovidData(lines);
                //await
                ProcessData();
            }
            else
            {
                SummaryTextBox.Text = "Operation cancelled.";
            }
        }

        private void ProcessData()
        {
            var state = DefaultStateSelector;
            if (StateComboBox.SelectedValue != null) state = StateComboBox.SelectedValue.ToString();
            var stateCovidData = DataCreator.GetStateCovidData(state);
            if (LoadedDataCollection.Count > 0)
                HandleExistingFileLoading(stateCovidData);
            else
                LoadedDataCollection = stateCovidData;

            DataController.SetObservableCollection(LoadedDataCollection);
            CreateNewReportSummary();
        }

        private async void HandleExistingFileLoading(CovidDataCollection covidCollection)
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

            if (result == Merge) MergeFile(covidCollection);
        }

        private async void MergeFile(CovidDataCollection covidCollection)
        {
            foreach (var currCovidData in covidCollection)
                if (LoadedDataCollection.Any(covidData => covidData.Date == currCovidData.Date))
                    await HandleDuplicateDay(currCovidData);
                else
                    LoadedDataCollection.Add(currCovidData);

            DataController.SetObservableCollection(LoadedDataCollection);
            CreateNewReportSummary();
        }

        private async Task HandleDuplicateDay(CovidData currCovidData)
        {
            var result = await ShowDuplicateDayDialog();

            if (result == Replace)
                ReplaceDuplicateDay(currCovidData);
            else if (result == Merge) MergeDuplicateDay(currCovidData);
        }

        private void ReplaceDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = LoadedDataCollection.First(covidData =>
                covidData.Date == duplicateDay);
            var index = LoadedDataCollection.IndexOf(day);
            LoadedDataCollection[index] = currCovidData;
        }

        private void MergeDuplicateDay(CovidData currCovidData)
        {
            var duplicateDay = currCovidData.Date;
            var day = LoadedDataCollection.First(covidData => covidData.Date == duplicateDay);
            var index = LoadedDataCollection.IndexOf(day);
            day.PositiveCasesIncrease += currCovidData.PositiveCasesIncrease;
            day.NegativeCasesIncrease += currCovidData.NegativeCasesIncrease;
            day.DeathNumbers += currCovidData.DeathNumbers;
            day.HospitalizedNumbers += currCovidData.HospitalizedNumbers;
            LoadedDataCollection[index] = day;
        }

        private static IAsyncOperation<ContentDialogResult> ShowDuplicateDayDialog()
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
            SummaryTextBox.Text =
                covidFormatter.FormatGeneralData(UpperBoundaryLimit, LowerBoundaryLimit, BinSize);
            SummaryTextBox.Text += covidFormatter.FormatMonthlyData(stateMonthData);
        }

        private async void ErrorLines_Click(object sender, RoutedEventArgs e)
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

        private static async Task<string[]> GetFileLines(StorageFile file)
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
        ///     The Covid Data Controller
        /// </summary>
        public CovidDataController DataController { get; set; }

        /// <summary>
        ///     The bin size to be used for histogram
        /// </summary>
        public int BinSize { get; set; }

        private const ContentDialogResult Replace = ContentDialogResult.Primary;

        private const ContentDialogResult Merge = ContentDialogResult.Secondary;

        private const string FileHeader = "date, state, positiveCases, negativeCases, death, hospitalized";

        #endregion
    }
}