using System;
using System.Globalization;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.DataHandling

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
        ///     Formats general data about a <see cref="CovidDataCollection" /> object
        /// </summary>
        /// <param name="upperBoundary">the upper boundary of the threshold</param>
        /// <param name="lowerBoundary">the lower boundary of the threshold</param>
        /// <returns>
        ///     Formatted string about general <see cref="CovidDataCollection" /> information
        /// </returns>
        public string FormatGeneralData(int upperBoundary, int lowerBoundary)
        {
            var output = "";
            output +=
                $"First Positive Case in GA: {this.CovidRecords.FindFirstPositiveTest().Date.ToShortDateString()}{Environment.NewLine}";
            output += this.formatHighestPositiveDay();
            output += this.formatHighestNegativeDay();
            output += this.formatHighestTestDay();
            output += this.formatHighestDeathDay();
            output += this.formatHighestHospitalizedDay();
            output += this.formatHighestPositivePercentage();
            output += this.formatAveragePositiveTest();
            output += this.formatOverAllPositivityRate();
            output += this.formatBoundaries(upperBoundary, lowerBoundary);
            output += this.formatSegmentData(500);
            return output;
        }

        private string formatHighestPositiveDay()
        {
            var highestPositive = this.CovidRecords.FindHighestNumberOfPositiveCasesInSingleDay();
            return
                $"Highest Number of Positive Cases in a Single Day: {highestPositive.Date.ToShortDateString()} " +
                $"with {highestPositive.PositiveCasesIncrease:n0} cases{Environment.NewLine}";
        }

        private string formatHighestNegativeDay()
        {
            var highestNegative = this.CovidRecords.FindHighestNumberOfNegativeCasesInSingleDay();
            return
                $"Highest Number of Negative Cases in a Single Day: {highestNegative.Date} with " +
                $"{highestNegative.NegativeCasesIncrease:n0} cases{Environment.NewLine}";
        }

        private string formatHighestTestDay()
        {
            var highestTest = this.CovidRecords.FindHighestNumberOfTests();
            return
                $"Highest Number of Test in a Single Day: {highestTest.Date.ToShortDateString()} with " +
                $"{highestTest.TotalTest:n0} tests{Environment.NewLine}";
        }

        private string formatHighestDeathDay()
        {
            var highestDeath = this.CovidRecords.FindHighestNumberOfDeaths();
            return
                $"Highest Number of Deaths in a Single Day: {highestDeath.Date.ToShortDateString()} with " +
                $"{highestDeath.DeathNumbers:n0} deaths{Environment.NewLine}";
        }

        private string formatHighestHospitalizedDay()
        {
            var hospitalized = this.CovidRecords.FindHighestNumberOfHospitalizations();
            return
                "Highest Number of Hospitalized in a Single Day: " +
                $"{hospitalized.Date.ToShortDateString()} with {hospitalized.HospitalizedNumbers:n0} hospitalized{Environment.NewLine}";
        }

        private string formatHighestPositivePercentage()
        {
            var positive = this.CovidRecords.FindHighestPositivePercentage();
            return
                "Highest Positive Test Percentage in Single Day: " +
                $"{positive.Date.ToShortDateString()} with {Convert.ToDecimal($"{positive.OverallPositivePercentage:0.00}")}%" +
                $"{Environment.NewLine}";
        }

        private string formatAveragePositiveTest()
        {
            var average = this.CovidRecords.FindAveragePositiveCasesSinceFirstPositive();
            return
                "Average Positive Test Per Day Since First Positive: " +
                $"{Convert.ToDecimal($"{average: 0.00}")} cases per day{Environment.NewLine}";
        }

        private string formatOverAllPositivityRate()
        {
            var rate = this.CovidRecords.FindOverallPositivityRate();
            return $"Overall Positive rate: {Convert.ToDecimal($"{rate:0.00}")}%{Environment.NewLine}";
        }

        private string formatBoundaries(int upperLimit, int lowerLimit)
        {
            var output = "";
            output += $"Number of Days With Over {upperLimit:n0} Positive Cases: " +
                      $"{this.CovidRecords.FindNumberOfDaysWithCasesOverThreshold(upperLimit):n0} days{Environment.NewLine}";
            output += $"Number of Days With Less than {lowerLimit:n0} Positive Cases: " +
                      $"{this.CovidRecords.FindNumberOfDaysWithCasesUnderLowerThreshold(lowerLimit):n0} days{Environment.NewLine}";
            return output;
        }

        private string formatSegmentData(int segmentRange)
        {
            var summary = $"{Environment.NewLine}";
            var segmentCounts = this.CovidRecords.CountDaysByPositiveCasesSegments(segmentRange);
            var startCases = 0;
            var endCases = segmentRange;
            for (var i = 0; i < segmentCounts.Length; i++)
            {
                summary += this.generateSegmentString(startCases, endCases, segmentCounts[i]);

                startCases = endCases + 1;
                endCases = segmentRange * (i + 2);
            }

            return summary;
        }

        private string generateSegmentString(int startCases, int endCases, int segmentCount)
        {
            var minFormatted = this.padWithLeadingSpaces($"{startCases:n0}", 10);
            var maxFormatted = this.padWithLeadingSpaces($"{endCases:n0}", 9);
            var countFormatted = this.padWithLeadingSpaces($"{segmentCount:n0}", 9);
            var row = $"{minFormatted} -{maxFormatted}:{countFormatted}{Environment.NewLine}";

            return row;
        }

        private string padWithLeadingSpaces(string number, int totalLength)
        {
            var spaces = "";
            var numberSpaces = totalLength - number.Length;
            for (var i = 0; i < numberSpaces; i++)
            {
                spaces = spaces + " ";
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
                if (monthData.CovidRecords.Any())
                {
                    output +=
                        $"{Environment.NewLine}{Environment.NewLine}{DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames[i]} {monthlyData.Year} " +
                        $"({monthData.CovidRecords.Count} days of date):{Environment.NewLine}";
                    output += this.formatMonthlyHighestPositiveDay(monthData);
                    output += this.formatMonthlyLowestPositive(monthData);
                    output += this.formatMonthlyMostTestInDay(monthData);
                    output += this.formatLeastTestInDay(monthData);
                    output += this.formatMonthlyAverageTestPerDay(monthData);
                    output += this.formatAverageNumberOfTestPerDay(monthData);
                }
                else
                {
                    output +=
                        $"{Environment.NewLine}{Environment.NewLine}{DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames[i]}" +
                        $"{Environment.NewLine}";
                }
            }

            return output;
        }

        private string formatDay(CovidData covidData)
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

        private string formatMonthlyHighestPositiveDay(CovidDataCollection monthData)
        {
            var highest = monthData.FindHighestNumberOfPositiveCasesInSingleDay();
            return
                $"Highest Positive Cases: {highest.PositiveCasesIncrease:n0} occurred on the {this.formatDay(highest)}{Environment.NewLine}";
        }

        private string formatMonthlyLowestPositive(CovidDataCollection monthData)
        {
            var lowest = monthData.FindLowestPositiveCases();
            return
                $"Lowest Positive Cases: {lowest.PositiveCasesIncrease:n0} occurred on {this.formatDay(lowest)}{Environment.NewLine}";
        }

        private string formatMonthlyMostTestInDay(CovidDataCollection monthData)
        {
            var mostTest = monthData.FindHighestNumberOfTests();
            return
                $"Most Test In Single Day: {mostTest.TotalTest:n0} occurred on {this.formatDay(mostTest)}{Environment.NewLine}";
        }

        private string formatLeastTestInDay(CovidDataCollection monthData)
        {
            var leastTest = monthData.FindLowestTotalCases();
            return
                $"Least Test in Single Day: {leastTest.TotalTest:n0} occurred on {this.formatDay(leastTest)}{Environment.NewLine}";
        }

        private string formatMonthlyAverageTestPerDay(CovidDataCollection monthData)
        {
            var average = monthData.FindAveragePositiveCasesSinceFirstPositive();
            return
                $"Average Positive Test Per Day Since First Positive: {Convert.ToDecimal($"{average: 0.00}")} cases per day" +
                $"{Environment.NewLine}";
        }

        private string formatAverageNumberOfTestPerDay(CovidDataCollection monthData)
        {
            var average = monthData.FindAverageNumberOfTestPerDay();
            return
                $"Average Number of Test Per Day: {Convert.ToDecimal($"{average:0.00}")} test per day{Environment.NewLine}";
        }

        #endregion
    }
}