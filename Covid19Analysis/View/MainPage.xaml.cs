using System;
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
            this.FilePath = "";
            this.UpperBoundaryLimit = GetBoundariesContentDialog.UpperBoundaryDefault;
            this.LowerBoundaryLimit = GetBoundariesContentDialog.LowerBoundaryDefault;

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
                this.UpperBoundaryLimit = boundaryContentDialog.UpperBoundary;
                this.LowerBoundaryLimit = boundaryContentDialog.LowerBoundary;
            }
        }

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail, SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                if (this.FilePath.Equals(file.Path))
                {
                    await this.handleDuplicateFile(file);
                }
                else
                {
                    this.FilePath = file.Path;
                    await this.processFile(file);
                }
            }
            else
            {
                this.SummaryTextBox.Text = "Operation cancelled.";
            }
        }

        private async Task handleDuplicateFile(StorageFile file)
        {
            var loadingDialog = new ContentDialog() {
                Title = "File Already In Use",
                Content = "Do You Wish To Replace Or Merge This File?",
                PrimaryButtonText = "Replace",
                SecondaryButtonText = "Merge"
            };

            var result = await loadingDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.FilePath = file.Path;
                await this.processFile(file);
            }
        }

        private async Task processFile(StorageFile file)
        {
            var lines = await getFileLines(file);
            var dataCreator = new CovidDataCreator(lines);
            dataCreator.CreateCovidData();
            this.showErrorDialog(dataCreator);
            var gaCovidData = dataCreator.GetStateCovidData("GA");
            var gaMonthData = new MonthlyCovidDataCollection(gaCovidData);
            var covidFormatter = new CovidDataFormatter(gaCovidData);
            this.SummaryTextBox.Text = covidFormatter.FormatGeneralData(this.UpperBoundaryLimit, this.LowerBoundaryLimit);
            this.SummaryTextBox.Text += covidFormatter.FormatMonthlyData(gaMonthData);
        }

        private async void showErrorDialog(CovidDataCreator dataCreator)
        {
            if (dataCreator.ErrorLines.Count != 0)
            {
                var errorDialog = new ContentDialog {
                    Title = "Lines With Errors",
                    Content = dataCreator.ErrorLinesToString(),
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
        ///     The file loaded into the application
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        ///     The upper boundary limit of the threshold
        /// </summary>
        public int UpperBoundaryLimit { get; set; }

        /// <summary>
        ///     the lower boundary limit of the threshold
        /// </summary>
        public int LowerBoundaryLimit { get; set; }

        #endregion

    }
}