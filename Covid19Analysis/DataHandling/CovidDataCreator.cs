using System;
using System.Collections.Generic;
using System.Globalization;
using Covid19Analysis.Model;

namespace Covid19Analysis.DataHandling
{
    /// <summary>
    ///     Class that handles creating <see cref="CovidData" /> from a file
    /// </summary>
    public class CovidDataCreator
    {
        #region Properties

        /// <summary>
        ///     The list of <see cref="CovidData" />
        /// </summary>
        public List<CovidData> CovidData { get; }

        /// <summary>
        ///     The String containing all lines with errors
        /// </summary>
        public Dictionary<int, string> ErrorLines { get; set; }

        private const int DateField = 0;
        private const int StateField = 1;
        private const int PositiveIncreaseField = 2;
        private const int NegativeIncreaseFiled = 3;
        private const int DeathNumberField = 4;
        private const int HospitalizedField = 5;

        #endregion

        #region Constructors

        /// <summary>
        ///     Instantiates a new <see cref="CovidDataCreator" /> class object
        /// </summary>
        public CovidDataCreator()
        {
            this.CovidData = new List<CovidData>();
            this.ErrorLines = new Dictionary<int, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates CovidData from a .csv file
        /// </summary>
        /// <param name="fileLines">
        ///     The lines of the files to be converted
        ///     Precondition: filelines != null
        /// </param>
        public void CreateCovidData(string[] fileLines)
        {
            if (fileLines == null)
            {
                throw new ArgumentException(nameof(fileLines));
            }
            for (var i = 1; i < fileLines.Length; i++)
            {
                var line = fileLines[i].Split(",");
                try
                {
                    var covidData = new CovidData(
                        DateTime.ParseExact(line[DateField], "yyyyMMdd", CultureInfo.InvariantCulture),
                        line[StateField],
                        this.FixNegativeInput(int.Parse(line[PositiveIncreaseField])),
                        this.FixNegativeInput(int.Parse(line[NegativeIncreaseFiled])),
                        this.FixNegativeInput(int.Parse(line[DeathNumberField])),
                        this.FixNegativeInput(int.Parse(line[HospitalizedField])));
                    this.CovidData.Add(covidData);
                }
                catch (Exception)
                {
                    this.ErrorLines.Add(i, fileLines[i]);
                }
            }
        }

        private int FixNegativeInput(int number)
        {
            var fixedNumber = number;
            if (fixedNumber < 0)
            {
                fixedNumber = Math.Abs(fixedNumber);
            }

            return fixedNumber;
        }

        /// <summary>
        ///     Pulls CovidData for GA from the list
        /// </summary>
        /// <param name="state"> The state for the given Data</param>
        /// <returns>
        ///     A <see cref="CovidDataCollection" /> for the state of GA
        /// </returns>
        public CovidDataCollection GetStateCovidData(string state)
        {
            var stateData = new CovidDataCollection();
            foreach (var currData in this.CovidData)
            {
                if (currData.State == state)
                {
                    stateData.Add(currData);
                }
            }

            return stateData;
        }

        /// <summary>
        ///     Returns the error lines in a string
        /// </summary>
        /// <returns>
        ///     The Error lines in a string
        /// </returns>
        public string ErrorLinesToString()
        {
            var output = "";
            foreach (var currkey in this.ErrorLines.Keys)
            {
                output += $"Line {currkey}: {this.ErrorLines[currkey]}{Environment.NewLine}";
            }

            return output;
        }

        #endregion
    }
}