using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     The AddCovidDataContentDialog
    /// </summary>
    public sealed partial class AddCovidDataContentDialog
    {
        /// <summary>
        ///     Instantiates a new <see cref="AddCovidDataContentDialog"/> object
        /// </summary>
        public AddCovidDataContentDialog()
        {
            this.InitializeComponent();
        }

        #region Properties

        /// <summary>
        ///     The Date of the covid data
        /// </summary>
        public DateTime DateDate { get; private set; }

        /// <summary>
        ///     The State of the covid data
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        ///     the number of positives for the covid data
        /// </summary>
        public int PositiveCaseIncrease { get; private set; }

        /// <summary>
        ///     the number of negative cases for the coivd data
        /// </summary>
        public int NegativeCaseIncrease { get; private set; }

        /// <summary>
        ///     the number of deaths for the covid data
        /// </summary>
        public int DeathNumbers { get; private set; }

        /// <summary>
        ///     the number of hospitalized for the covid data
        /// </summary>
        public int HospitalizedNumbers { get; private set; }

        #endregion

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            this.DateDate = this.DataDatePicker.Date.Date;
            this.State = this.StateTextBox.Text;
            this.PositiveCaseIncrease = int.Parse(this.PositiveCasesTextBox.Text);
            this.NegativeCaseIncrease = int.Parse(this.NegativeCasesTextBox.Text);
            this.DeathNumbers = int.Parse(this.DeathsTextBox.Text);
            this.HospitalizedNumbers = int.Parse(this.HospitalizedTextBox.Text);

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void PositiveCasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex numberRegex = new Regex("\\d+");
            if (!numberRegex.IsMatch(this.PositiveCasesTextBox.Text))
            {
                this.ErrorLabel.Visibility = Visibility.Visible;
                this.ErrorLabel.Text = "Numbers must be greater than or equal to zero";
            }
            else
            {
                this.ErrorLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
