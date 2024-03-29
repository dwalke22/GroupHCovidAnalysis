﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19Analysis.DataHandling;

namespace Covid19Analysis.Model

{
    /// <summary>
    ///     The class that formats <see cref="CovidDataCollection" /> into an
    ///     easy to read format
    /// </summary>
    public class CovidDataFormatter
    {
        #region Properties

        /// <summary>
        ///     The <see cref="CovidDataCollection" /> to be formatted
        /// </summary>
        public CovidDataCollection CovidRecords { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CovidDataFormatter" /> class
        /// </summary>
        /// <param name="covidRecords">
        ///     The collection of <see cref="CovidDataCollection" /> to be formatted
        /// </param>
        public CovidDataFormatter(CovidDataCollection covidRecords)
        {
            this.CovidRecords = covidRecords ?? throw new ArgumentNullException(nameof(covidRecords));
        }

        #endregion

        #region Mehtods

        /// <summary>
        ///     Returns the error lines in a string
        /// </summary>
        /// <param name="creator">
        ///     The CovidDataCrator that holds the error lines
        ///     Precondition: creator != null
        /// </param>
        /// <returns>
        ///     The Error lines in a string
        /// </returns>
        public string ErrorLinesToString(CovidDataCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentException(nameof(creator));
            }

            var output = "";
            foreach (var currkey in creator.ErrorLines.Keys)
            {
                output += $"Line {currkey}: {creator.ErrorLines[currkey]}{Environment.NewLine}";
            }

            return output;
        }

        /// <summary>
        ///     Formats general data about a <see cref="CovidDataCollection" /> object
        /// </summary>
        /// <param name="upperBoundary">the upper boundary of the threshold</param>
        /// <param name="lowerBoundary">the lower boundary of the threshold</param>
        /// <param name="binsize">the size of the bin for histogram</param>
        /// <returns>
        ///     Formatted string about general <see cref="CovidDataCollection" /> information
        /// </returns>
        public string FormatGeneralData(int upperBoundary, int lowerBoundary, int binsize)
        {
            var output = "";
            if (this.CovidRecords.Count > 0)
            {
                output +=
                    $"First Positive Case in GA: {this.CovidRecords.FindFirstPositiveTest().Date.ToShortDateString()}{Environment.NewLine}";
                output += this.FormatHighestPositiveDay();
                output += this.FormatHighestNegativeDay();
                output += this.FormatHighestTestDay();
                output += this.FormatHighestDeathDay();
                output += this.FormatHighestHospitalizedDay();
                output += this.FormatHighestPositivePercentage();
                output += this.FormatHighestCurrentHospitalization();
                output += this.FormatAveragePositiveTest();
                output += this.FormatOverAllPositivityRate();
                output += this.FormatAverageCurrentHospitalizations();
                output += this.FormatBoundaries(upperBoundary, lowerBoundary);
                output += this.FormatHistogram(binsize);
            }
            else
            {
                output = "No Data Available";
            }
            return output;
        }

        private string FormatHighestPositiveDay()
        {
            var highestPositive = this.CovidRecords.FindHighestNumberOfPositiveCasesInSingleDay();
            return
                $"Highest Number of Positive Cases in a Single Day: {highestPositive.Date.ToShortDateString()} " +
                $"with {highestPositive.PositiveCasesIncrease:n0} cases{Environment.NewLine}";
        }

        private string FormatHighestNegativeDay()
        {
            var highestNegative = this.CovidRecords.FindHighestNumberOfNegativeCasesInSingleDay();
            return
                $"Highest Number of Negative Cases in a Single Day: {highestNegative.Date} with " +
                $"{highestNegative.NegativeCasesIncrease:n0} cases{Environment.NewLine}";
        }

        private string FormatHighestTestDay()
        {
            var highestTest = this.CovidRecords.FindHighestNumberOfTests();
            return
                $"Highest Number of Test in a Single Day: {highestTest.Date.ToShortDateString()} with " +
                $"{highestTest.TotalTest:n0} tests{Environment.NewLine}";
        }

        private string FormatHighestDeathDay()
        {
            var highestDeath = this.CovidRecords.FindHighestNumberOfDeaths();
            return
                $"Highest Number of Deaths in a Single Day: {highestDeath.Date.ToShortDateString()} with " +
                $"{highestDeath.DeathNumbers:n0} deaths{Environment.NewLine}";
        }

        private string FormatHighestHospitalizedDay()
        {
            var hospitalized = this.CovidRecords.FindHighestNumberOfHospitalizations();
            return
                "Highest Number of Hospitalized in a Single Day: " +
                $"{hospitalized.Date.ToShortDateString()} with {hospitalized.HospitalizedNumbers:n0} hospitalized{Environment.NewLine}";
        }

        private string FormatHighestPositivePercentage()
        {
            var positive = this.CovidRecords.FindHighestPositivePercentage();
            return
                "Highest Positive Test Percentage in Single Day: " +
                $"{positive.Date.ToShortDateString()} with {Convert.ToDecimal($"{positive.OverallPositivePercentage:0.00}")}%" +
                $"{Environment.NewLine}";
        }

        private string FormatHighestCurrentHospitalization()
        {
            var highestCurrentHosp = this.CovidRecords.FindHighestCurrentHospitalization();
            return $"Highest Current Hospitalizations: {highestCurrentHosp.Date.ToShortDateString()} with " +
                $"{highestCurrentHosp.CurrentHospitalized:n0} currently hospitalized{Environment.NewLine}";
        }

        private string FormatAveragePositiveTest()
        {
            var average = this.CovidRecords.FindAveragePositiveCasesSinceFirstPositive();
            return
                "Average Positive Test Per Day Since First Positive: " +
                $"{Convert.ToDecimal($"{average: 0.00}"):N} cases per day{Environment.NewLine}";
        }

        private string FormatOverAllPositivityRate()
        {
            var rate = this.CovidRecords.FindOverallPositivityRate();
            return $"Overall Positive rate: {Convert.ToDecimal($"{rate:0.00}"):N}%{Environment.NewLine}";
        }

        private string FormatAverageCurrentHospitalizations()
        {
            var averageCurrHopsit = this.CovidRecords.FindAverageCurrentHospitalization();
            return $"Average Current Hospitalizations: {Convert.ToDecimal($"{averageCurrHopsit:0.00}"):n} " +
                   $"hospitalizations per day{Environment.NewLine}";
        }

        private string FormatBoundaries(int upperLimit, int lowerLimit)
        {
            var output = "";
            output += $"Number of Days With Over {upperLimit:n0} Positive Cases: " +
                      $"{this.CovidRecords.FindNumberOfDaysWithCasesOverThreshold(upperLimit):n0} days{Environment.NewLine}";
            output += $"Number of Days With Less than {lowerLimit:n0} Positive Cases: " +
                      $"{this.CovidRecords.FindNumberOfDaysWithCasesUnderLowerThreshold(lowerLimit):n0} days{Environment.NewLine}";
            return output;
        }

        // ReSharper disable once UnusedMember.Local
        private string GenerateSegmentString(int startCases, int endCases, int segmentCount)
        {
            var minFormatted = this.padWithLeadingSpaces($"{startCases:n0}", 10);
            var maxFormatted = this.padWithLeadingSpaces($"{endCases:n0}", 9);
            var countFormatted = this.padWithLeadingSpaces($"{segmentCount:n0}", 9);
            var row = $"{minFormatted} -{maxFormatted}:{countFormatted}{Environment.NewLine}";

            return row;
        }

        private string FormatHistogram(int binsize)
        {
            var formattedHistogram = $"{Environment.NewLine}";
            var lowerBound = 0;
            var upperBound = binsize;
            var shouldHistogramStop = false;
            while (!shouldHistogramStop)
            {
                formattedHistogram +=
                    $"{increaseByOneForNonZero(lowerBound)} - {upperBound}: {this.CovidRecords.FindPositiveCasesBetweenValues(increaseByOneForNonZero(lowerBound), upperBound)}" +
                    Environment.NewLine;
                shouldHistogramStop = this.CovidRecords.BoundsContainHighestIncrease(lowerBound, upperBound);
                lowerBound += binsize;
                upperBound += binsize;
            }

            return formattedHistogram;
        }

        private int increaseByOneForNonZero(int bound)
        {
            if (bound != 0)
            {
                return bound + 1;
            }

            return bound;
        }


        private string padWithLeadingSpaces(string number, int totalLength)
        {
            var spaces = "";
            var numberSpaces = totalLength - number.Length;
            for (var i = 0; i < numberSpaces; i++)
            {
                spaces += " ";
            }

            return spaces + number;
        }

        /// <summary>
        ///     Formats the an output of monthly <see cref="CovidData" /> information
        /// </summary>
        /// <param name="monthlyData">
        ///     Array of monthly <see cref="MonthlyCovidDataCollection" />
        /// </param>
        /// <returns>
        ///     The formatted output
        /// </returns>
        public string FormatMonthlyData(MonthlyCovidDataCollection monthlyData)
        {
            var output = "";
            for (var i = 0; i < monthlyData.MonthlyCollection.Length; i++)
            {
                var monthData = monthlyData.MonthlyCollection[i];
                if (monthData.Count > 0)
                {
                    output +=
                        $"{Environment.NewLine}{Environment.NewLine}{DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames[i]} {monthlyData.Year} " +
                        $"({monthData.Count} days of date):{Environment.NewLine}";
                    output += this.FormatMonthlyHighestPositiveDay(monthData);
                    output += this.FormatMonthlyLowestPositive(monthData);
                    output += this.FormatMonthlyMostTestInDay(monthData);
                    output += this.FormatLeastTestInDay(monthData);
                    output += this.FormatHighestCurrentHospitalization(monthData);
                    output += this.FormatLeastCurrentHospitalization(monthData);
                    output += this.formatMonthlyAverageTestPerDay(monthData);
                    output += this.formatAverageNumberOfTestPerDay(monthData);
                }
            }

            return output;
        }

        private string formatDayOrdinals(CovidData covidData)
        {
            var dayString = covidData.Date.Day.ToString();
            if (dayString.Equals("11|12|13"))
            {
                return dayString + "th";
            }

            if (dayString.EndsWith("1"))
            {
                dayString += "st";
            }
            else if (dayString.EndsWith("2"))
            {
                dayString += "nd";
            }
            else if (dayString.EndsWith("3"))
            {
                dayString += "rd";
            }
            else
            {
                dayString += "th";
            }

            return dayString;
        }

        private string FormatMonthlyHighestPositiveDay(CovidDataCollection monthData)
        {
            var highestPositive = monthData.FindHighestNumberOfPositiveCasesInSingleDay().PositiveCasesIncrease;
            var daysWithHighest = monthData
                                           .Where(covidData => covidData.PositiveCasesIncrease == highestPositive)
                                           .Select(covidData => covidData).ToList();
            var daysString = this.FormatMultipuleDays(daysWithHighest);
            return
                $"Highest Positive Cases: {highestPositive:n0} occurred on the {daysString}{Environment.NewLine}";
        }

        private string FormatMultipuleDays(List<CovidData> days)
        {
            var daysString = string.Empty;
            foreach (var day in days)
            {
                var index = days.IndexOf(day);
                if (index == days.Count - 1)
                {
                    daysString += $"and {this.formatDayOrdinals(day)}";
                }
                else
                {
                    daysString += $"{this.formatDayOrdinals(day)}, ";
                }
            }

            return daysString;
        }

        private string FormatMonthlyLowestPositive(CovidDataCollection monthData)
        {
            var lowestPositives = monthData.FindLowestPositiveCases().PositiveCasesIncrease;
            var daysWithLowest = monthData
                                          .Where(covidData => covidData.PositiveCasesIncrease == lowestPositives)
                                          .Select(covidData => covidData).ToList();
            var daysString = this.FormatMultipuleDays(daysWithLowest);
            return
                $"Lowest Positive Cases: {lowestPositives:n0} occurred on {daysString}{Environment.NewLine}";
        }

        private string FormatMonthlyMostTestInDay(CovidDataCollection monthData)
        {
            var mostTest = monthData.FindHighestNumberOfTests().TotalTest;
            var daysWithMostTests = monthData
                                             .Where(covidData => covidData.TotalTest == mostTest)
                                             .Select(covidData => covidData).ToList();
            var daysString = this.FormatMultipuleDays(daysWithMostTests);
            return
                $"Most Test In Single Day: {mostTest:n0} occurred on {daysString}{Environment.NewLine}";
        }

        private string FormatLeastTestInDay(CovidDataCollection monthData)
        {
            var leastTest = monthData.FindLowestTotalCases().TotalTest;
            var daysWithLeastTests = monthData
                                              .Where(covidData => covidData.TotalTest == leastTest)
                                              .Select(covidData => covidData).ToList();
            var daysString = this.FormatMultipuleDays(daysWithLeastTests);
            return
                $"Least Test in Single Day: {leastTest:n0} occurred on {daysString}{Environment.NewLine}";
        }

        private string FormatHighestCurrentHospitalization(CovidDataCollection monthData)
        {
            try
            {
                var highestCurHops = monthData.FindHighestCurrentHospitalization().CurrentHospitalized;
                var daysWithHighest = monthData.Where(covidData => covidData.CurrentHospitalized == highestCurHops)
                                               .Select(covidData => covidData).ToList();
                var daysString = this.FormatMultipuleDays(daysWithHighest);
                return
                    $"Highest Current Hospitalizations: {highestCurHops:n0} occurred on {daysString}{Environment.NewLine}";
            }
            catch (Exception)
            {
                return $"No sufficient data on the highest hospitalizations{Environment.NewLine}";
            }
        }

        private string FormatLeastCurrentHospitalization(CovidDataCollection monthData)
        {
            try
            {
                var leastCurrHosp = monthData.FindLowestCurrentHospitalization().CurrentHospitalized;
                var daysWithLowest = monthData.Where(covidData => covidData.CurrentHospitalized == leastCurrHosp)
                                              .Select(covidData => covidData).ToList();
                var daysString = this.FormatMultipuleDays(daysWithLowest);
                return $"Lowest Current Hospitalizations: {leastCurrHosp:n0} occurred on {daysString}{Environment.NewLine}";
            }
            catch (Exception)
            {
                return $"No sufficient data on the lowest hospitalizations{Environment.NewLine}";
            }
        }

        private string formatMonthlyAverageTestPerDay(CovidDataCollection monthData)
        {
            var average = monthData.FindAveragePositiveCasesSinceFirstPositive();
            return
                $"Average Positive Test Per Day Since First Positive: {Convert.ToDecimal($"{average: 0.00}"):N} cases per day" +
                $"{Environment.NewLine}";
        }

        private string formatAverageNumberOfTestPerDay(CovidDataCollection monthData)
        {
            var average = monthData.FindAverageNumberOfTestPerDay();
            return
                $"Average Number of Test Per Day: {Convert.ToDecimal($"{average:0.00}"):N} test per day{Environment.NewLine}";
        }

        #endregion
    }
}