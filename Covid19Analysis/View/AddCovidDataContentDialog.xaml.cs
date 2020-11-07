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

        /// <summary>
        ///     The number of currently hospitalized
        /// </summary>
        public int CurrHospitalized { get; private set; }

        private Regex NumberRegex => new Regex("\\d+");

        #endregion

        #region Constructors

        /// <summary>
        ///     Instantiates a new <see cref="AddCovidDataContentDialog" /> object
        /// </summary>
        public AddCovidDataContentDialog()
        {
            this.InitializeComponent();
            this.DateDate = new DateTime();
            this.State = string.Empty;
            this.PositiveCaseIncrease = 0;
            this.NegativeCaseIncrease = 0;
            this.DeathNumbers = 0;
            this.HospitalizedNumbers = 0;
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.DateDate = this.dataDatePicker.Date.Date;
            this.State = this.stateComboBox.SelectionBoxItem.ToString();
            this.PositiveCaseIncrease = int.Parse(this.positiveCasesTextBox.Text);
            this.NegativeCaseIncrease = int.Parse(this.negativeCasesTextBox.Text);
            this.DeathNumbers = int.Parse(this.deathsTextBox.Text);
            this.HospitalizedNumbers = int.Parse(this.hospitalizedTextBox.Text);
            this.CurrHospitalized = int.Parse(this.currHospitalizedTextBox.Text);
        }

        private void PositiveCasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkForPositiveNumbers(this.positiveCasesTextBox);
        }

        private void checkForPositiveNumbers(TextBox textBox)
        {
            if (!this.NumberRegex.IsMatch(textBox.Text))
            {
                this.errorLabel.Visibility = Visibility.Visible;
                this.errorLabel.Text = "Numbers must be greater than or equal to zero";
            }
            else
            {
                this.errorLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void NegativeCasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkForPositiveNumbers(this.negativeCasesTextBox);
        }

        private void DeathsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkForPositiveNumbers(this.deathsTextBox);
        }

        private void HospitalizedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkForPositiveNumbers(this.hospitalizedTextBox);
        }

        private void currHospitalizedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkForPositiveNumbers(this.currHospitalizedTextBox);
        }
        #endregion

    }
}