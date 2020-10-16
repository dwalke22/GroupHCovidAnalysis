﻿using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     The GetBoundariesContentDialog class
    /// </summary>
    public sealed partial class GetBoundariesContentDialog
    {
        #region Constructors

        /// <summary>
        ///     Instantiates a new <see cref="GetBoundariesContentDialog" /> object
        /// </summary>
        public GetBoundariesContentDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Data members

        /// <summary>
        ///     The upper limit default value for the upper limit
        /// </summary>
        public const int UpperBoundaryDefault = 2500;

        /// <summary>
        ///     The lower limit default value for the lower limit
        /// </summary>
        public const int LowerBoundaryDefault = 1000;

        #endregion

        #region Properties

        /// <summary>
        ///     The Upper Boundary of the threshold
        /// </summary>
        public int UpperBoundary { get; private set; }

        /// <summary>
        ///     The Lower Boundary of the threshold
        /// </summary>
        public int LowerBoundary { get; private set; }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var upperBoundary = UpperBoundaryTextBox.Text;
            var lowerBoundary = LowerBoundaryTextBox.Text;
            var numberRegex = new Regex("\\d+");
            if (string.IsNullOrEmpty(upperBoundary) | !numberRegex.IsMatch(upperBoundary))
                UpperBoundary = UpperBoundaryDefault;
            else
                UpperBoundary = int.Parse(UpperBoundaryTextBox.Text);

            if (string.IsNullOrEmpty(lowerBoundary) | !numberRegex.IsMatch(lowerBoundary))
                LowerBoundary = LowerBoundaryDefault;
            else
                LowerBoundary = int.Parse(LowerBoundaryTextBox.Text);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            UpperBoundary = UpperBoundaryDefault;
            LowerBoundary = LowerBoundaryDefault;
        }

        #endregion
    }
}