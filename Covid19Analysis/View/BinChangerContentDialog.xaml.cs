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

        #region Constructors

        /// <summary>
        ///     Instantiates a new BinChangerContentDialog
        /// </summary>
        public BinChangerContentDialog()
        {
            InitializeComponent();
            BinSize = DefaultBinSize;
            IsPrimaryButtonEnabled = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The BinSize to be used for formatting
        /// </summary>
        public int BinSize { get; private set; }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            BinSize = int.Parse(BinSizeTextBox.Text);
        }

        private void binSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var numRegex = new Regex("\\d+");
            if (!numRegex.IsMatch(BinSizeTextBox.Text))
            {
                ErrorLabel.Text = "Number must be greater than zero";
                ErrorLabel.Visibility = Visibility.Visible;
                BinSizeTextBox.Text = string.Empty;
            }
            else
            {
                ErrorLabel.Visibility = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(BinSizeTextBox.Text)) IsPrimaryButtonEnabled = true;
        }

        #endregion
    }
}