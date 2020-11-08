using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     The BinChanger Dialog class
    /// </summary>
    public sealed partial class BinChangerContentDialog
    {
        #region Data members

        /// <summary>
        ///     The default bin size
        /// </summary>
        public const int DefaultBinSize = 500;

        #endregion

        #region Properties

        /// <summary>
        ///     The BinSize to be used for formatting
        /// </summary>
        public int BinSize { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Instantiates a new BinChangerContentDialog
        /// </summary>
        public BinChangerContentDialog()
        {
            this.InitializeComponent();
            this.BinSize = DefaultBinSize;
            IsPrimaryButtonEnabled = false;
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.BinSize = int.Parse(this.binSizeTextBox.Text);
        }

        private void binSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var numRegex = new Regex("\\d+");
            if (!numRegex.IsMatch(this.binSizeTextBox.Text))
            {
                this.ErrorLabel.Text = "Number must be greater than zero";
                this.ErrorLabel.Visibility = Visibility.Visible;
                this.binSizeTextBox.Text = string.Empty;
            }
            else
            {
                this.ErrorLabel.Visibility = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(this.binSizeTextBox.Text))
            {
                IsPrimaryButtonEnabled = true;
            }
        }

        #endregion
    }
}