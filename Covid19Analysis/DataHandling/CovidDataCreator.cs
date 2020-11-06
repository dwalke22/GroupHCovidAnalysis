﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Covid19Analysis.Model;

namespace Covid19Analysis.DataHandling
{
    /// <summary>
    ///     Class that handles creating <see cref="CovidData" /> from a file
    /// </summary>
    public class CovidDataCreator
    {
        #region Data members

        private const int NumberOfFields = 7;
        private const int DateField = 0;
        private const int StateField = 1;
        private const int PositiveIncreaseField = 2;
        private const int NegativeIncreaseField = 3;
        private const int CurrHospitalizedField = 4;
        private const int HospitalizedField = 5;
        private const int DeathNumberField = 6;

        #endregion

        #region Properties

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

            this.CovidData.Clear();
            this.ErrorLines.Clear();
            for (var i = 1; i < fileLines.Length; i++)
            {
                var line = fileLines[i].Split(",");
                try
                {
                    line = fillMissingFields(line);
                    var dateTime = DateTime.ParseExact(line[DateField], "M/dd/yyyy", CultureInfo.InvariantCulture);
                    var state = line[StateField];
                    var positiveCasesIncrease = this.FixNegativeInput(int.Parse(line[PositiveIncreaseField]));
                    var negativeCasesIncrease = this.FixNegativeInput(int.Parse(line[NegativeIncreaseField]));
                    var currentHospitalized = this.FixNegativeInput(int.Parse(line[CurrHospitalizedField]));
                    var deathIncrease = this.FixNegativeInput(int.Parse(line[DeathNumberField]));
                    var hospitalizedIncrease = this.FixNegativeInput(int.Parse(line[HospitalizedField]));
                    var covidData = new CovidData(dateTime, state, positiveCasesIncrease, negativeCasesIncrease, currentHospitalized, deathIncrease, hospitalizedIncrease);
                    this.CovidData.Add(covidData);
                }
                catch (Exception)
                {
                    this.ErrorLines.Add(i, fileLines[i]);
                    System.Console.WriteLine(fileLines[i]);
                }
            }
        }

        private static string[] fillMissingFields(string[] line)
        {
            try
            {
                var newLine = new string[NumberOfFields];

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == null || line[i].Equals(string.Empty))
                    {
                        newLine[i] = "0";
                    }
                    else
                    {
                        newLine[i] = line[i];
                    }
                }

                return newLine;
            }
            catch (Exception)
            {
                System.Console.WriteLine(line);
                return line;
            }
        }

        private int FixNegativeInput(int number)
        {
            var fixedNumber = number;
            if (fixedNumber < 0)
            {
                fixedNumber = 0;
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
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<CovidDataCollection> DeserializeCovidData(StorageFile file)
        {
            this.CovidData.Clear();
            this.ErrorLines.Clear();

            var deserializer = new XmlSerializer(typeof(CovidDataCollection));

            var inStream = await file.OpenStreamForReadAsync();
            var covidDataCollection = (CovidDataCollection)deserializer.Deserialize(inStream);

            this.CovidData.AddRange(covidDataCollection);

            return covidDataCollection;
        }

        #endregion
    }
}