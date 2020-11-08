﻿using System;
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
        #region Constructors

        /// <summary>
        ///     Instantiates a new <see cref="AddCovidDataContentDialog" /> object
        /// </summary>
        public AddCovidDataContentDialog()
        {
            InitializeComponent();
            DateDate = new DateTime();
            State = string.Empty;
            PositiveCaseIncrease = 0;
            NegativeCaseIncrease = 0;
            DeathNumbers = 0;
            HospitalizedNumbers = 0;
        }

        #endregion

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

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DateDate = DataDatePicker.Date.Date;
            State = StateComboBox.SelectionBoxItem.ToString();
            PositiveCaseIncrease = int.Parse(PositiveCasesTextBox.Text);
            NegativeCaseIncrease = int.Parse(NegativeCasesTextBox.Text);
            DeathNumbers = int.Parse(DeathsTextBox.Text);
            HospitalizedNumbers = int.Parse(HospitalizedTextBox.Text);
            CurrHospitalized = int.Parse(CurrHospitalizedTextBox.Text);
        }

        private void PositiveCasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForPositiveNumbers(PositiveCasesTextBox);
        }

        /// <summary>
        ///     Checks for positive numbers.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        private void CheckForPositiveNumbers(TextBox textBox)
        {
            if (!NumberRegex.IsMatch(textBox.Text))
            {
                ErrorLabel.Visibility = Visibility.Visible;
                ErrorLabel.Text = "Numbers must be greater than or equal to zero";
            }
            else
            {
                ErrorLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void NegativeCasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForPositiveNumbers(NegativeCasesTextBox);
        }

        private void DeathsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForPositiveNumbers(DeathsTextBox);
        }

        private void HospitalizedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForPositiveNumbers(HospitalizedTextBox);
        }

        private void currHospitalizedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForPositiveNumbers(CurrHospitalizedTextBox);
        }

        #endregion
    }
}