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
        ///     The File lines read from the file
        /// </summary>
        public string[] FileLines { get; }

        /// <summary>
        ///     The list of <see cref="CovidData" />
        /// </summary>
        public List<CovidData> CovidData { get; }

        /// <summary>
        ///     The String containing all lines with errors
        /// </summary>
        public Dictionary<int, string> ErrorLines { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Instantiates a new <see cref="CovidDataCreator" /> class object
        /// </summary>
        /// <param name="fileLines">
        ///     The lines read from the file
        /// </param>
        public CovidDataCreator(string[] fileLines)
        {
            this.FileLines = fileLines ?? throw new ArgumentNullException(nameof(fileLines));
            this.CovidData = new List<CovidData>();
            this.ErrorLines = new Dictionary<int, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates CovidData from a .csv file
        /// </summary>
        public void CreateCovidData()
        {
            for (var i = 1; i < this.FileLines.Length; i++)
            {
                var line = this.FileLines[i].Split(",");
                try
                {
                    var covidData = new CovidData(
                        DateTime.ParseExact(line[0], "yyyyMMdd", CultureInfo.InvariantCulture),
                        line[1],
                        this.FixNegativeInput(int.Parse(line[2])),
                        this.FixNegativeInput(int.Parse(line[3])),
                        this.FixNegativeInput(int.Parse(line[4])),
                        this.FixNegativeInput(int.Parse(line[5])));
                    this.CovidData.Add(covidData);
                }
                catch (Exception)
                {
                    this.ErrorLines.Add(i, this.FileLines[i]);
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
            var gaData = new CovidDataCollection();
            foreach (var currData in this.CovidData)
            {
                if (currData.State == "GA")
                {
                    gaData.Add(currData);
                }
            }

            return gaData;
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