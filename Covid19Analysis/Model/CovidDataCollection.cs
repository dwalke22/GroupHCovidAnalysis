using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     CovidRecords class that stores a collection of Covid data for a state
    /// </summary>
    public class CovidDataCollection
    {
        #region Properties

        /// <summary>
        ///     The List of <see cref="CovidData" />
        /// </summary>
        public IList<CovidData> CovidRecords { get; set; }

        /// <summary>
        ///     The Count for the number of records in the collection
        /// </summary>
        public int Count => this.CovidRecords.Count;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CovidDataCollection" /> class
        /// </summary>
        public CovidDataCollection()
        {
            this.CovidRecords = new List<CovidData>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds a <see cref="CovidData" /> CovidData object to the list
        ///     Precondition:
        ///         data != null
        ///     PostCondition:
        ///         Count = Count@prev + 1 
        /// </summary>
        /// <param name="data">The data object to be added</param>
        /// <exception cref="NullReferenceException">data</exception>
        public void Add(CovidData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.CovidRecords.Add(data);
        }

        /// <summary>
        ///     Find the first date of a Positive Case
        /// </summary>
        /// <returns>
        ///     Returns the <see cref="DateTime" /> of the first positive case
        /// </returns>
        public DateTime FindFirstPositiveTest()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Collection contains no elements");
            }
            var earliestDate = this.CovidRecords.Where(covidData => covidData.PositiveCasesIncrease > 0)
                .OrderBy(covidData => covidData.Date).First().Date;
            return earliestDate;
        }

        /// <summary>
        ///     Finds and returns the record for most positive cases
        ///     in a single day
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> record for most positive cases
        ///     in a single day
        /// </returns>
        public CovidData FindHighestNumberOfPositiveCasesInSingleDay()
        {
            var highestPositive = this.CovidRecords.Where(covidData => covidData.OverallPositivePercentage > 0)
                .OrderByDescending(covidData => covidData.PositiveCasesIncrease).First();
            return highestPositive;
        }

        /// <summary>
        ///     Finds and returns data on the day with the most
        ///     negative cases in a single day
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> on the most negative cases
        ///     in a single day
        /// </returns>
        public CovidData FindHighestNumberOfNegativeCasesInSingleDay()
        {
            var highestNegatives = this.CovidRecords.OrderByDescending(covidData => covidData.NegativeCasesIncrease)
                                       .First();
            return highestNegatives;
        }

        /// <summary>
        ///     Finds the CovidData for the day with the most amount of tests
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the highest number of tests for a single day
        /// </returns>
        public CovidData FindHighestNumberOfTests()
        {
            var highestTest = this.CovidRecords.OrderByDescending(covidData => covidData.TotalTest).First();
            return highestTest;
        }

        /// <summary>
        ///     Finds the day with the highest number of deaths
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> for the day with the highest number of deaths
        /// </returns>
        public CovidData FindHighestNumberOfDeaths()
        {
            var highestDeaths = this.CovidRecords.OrderByDescending(covidData => covidData.DeathNumbers).First();
            return highestDeaths;
        }

        /// <summary>
        ///     Finds the highest number of hospitalizations for a single day
        /// </summary>
        /// <returns>
        ///     The <see cref="CovidData" /> with the highest number of hospitalizations
        /// </returns>
        public CovidData FindHighestNumberOfHospitalizations()
        {
            var highestHospitalized =
                this.CovidRecords.OrderByDescending(covidData => covidData.HospitalizedNumbers).First();
            return highestHospitalized;
        }

        /// <summary>
        ///     Finds the day with the highest positive percentage
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the highest percentage with
        ///     a positive cases
        /// </returns>
        public CovidData FindHighestPositivePercentage()
        {
            var highestPositive = this.CovidRecords.OrderByDescending(covidData => covidData.OverallPositivePercentage)
                                      .First();
            return highestPositive;
        }

        /// <summary>
        ///     Finds the average number of positive test per day since first positive
        /// </summary>
        /// <returns>
        ///     The average <see cref="double" /> of positive test per day
        /// </returns>
        public double FindAveragePositiveCasesSinceFirstPositive()
        {
            var averagePositive = this.CovidRecords.Average(covidData => covidData.PositiveCasesIncrease);
            return averagePositive;
        }

        /// <summary>
        ///     Finds the overall rate of positive cases
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Double" /> that represents the percentage of positive cases
        /// </returns>
        public double FindOverallPositivityRate()
        {
            var averageRate = this.CovidRecords.Average(covidData => covidData.OverallPositivePercentage);
            return averageRate;
        }

        /// <summary>
        ///     Finds the number of days with more positive than threshold
        /// </summary>
        /// <param name="casesThreshold">
        ///     The threshold to check for days this more than threshold positives
        /// </param>
        /// <returns>
        ///     The number <see cref="int" /> of days with more than 2500 positives
        /// </returns>
        public int FindNumberOfDaysWithCasesOverThreshold(int casesThreshold)
        {
            return this.CovidRecords.Count(data => data.PositiveCasesIncrease > casesThreshold);
        }

        /// <summary>
        ///     Finds the number of days with less than cases threshold
        /// </summary>
        /// <param name="casesThreshold">
        ///     The threshold to check for days with less than threshold positives
        /// </param>
        /// <returns>
        ///     The number <see cref="int" /> of days with less than 1000 positives
        /// </returns>
        public int FindNumberOfDaysWithCasesUnderLowerThreshold(int casesThreshold)
        {
            return this.CovidRecords.Where(covidData => covidData.PositiveCasesIncrease > 0)
                .Count(data => data.PositiveCasesIncrease < casesThreshold);
        }

        /// <summary>
        ///     Finds the day with the lowest number of Positive Cases
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the lowest number of positive cases
        /// </returns>
        public CovidData FindLowestPositiveCases()
        {
            var lowestPositive = this.CovidRecords.OrderBy(covidData => covidData.PositiveCasesIncrease).First();
            return lowestPositive;
        }

        /// <summary>
        ///     Finds the lowest number of total cases
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> that had the lowest number of total cases
        /// </returns>
        public CovidData FindLowestTotalCases()
        {
            var lowestTotalCases = this.CovidRecords.OrderBy(covidData => covidData.TotalTest).First();
            return lowestTotalCases;
        }

        /// <summary>
        ///     Finds the number of average tests per day
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Double" /> that represents the average number of tests per day
        /// </returns>
        public double FindAverageNumberOfTestPerDay()
        {
            var averageTotalTest = this.CovidRecords.Average(covidData => covidData.TotalTest);
            return averageTotalTest;
        }

        /// <summary>
        ///     Separates the <see cref="CovidDataCollection" /> into segments based on Positive cases
        /// </summary>
        /// <param name="segmentRange">The range of each segment</param>
        /// <returns>
        ///     An array that has the number of objects that fall into each range
        /// </returns>
        public int[] CountDaysByPositiveCasesSegments(int segmentRange)
        {
            if (segmentRange < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            var maxPositive = this.FindHighestNumberOfPositiveCasesInSingleDay().PositiveCasesIncrease;
            var ceiling = (double) maxPositive / segmentRange;
            var segments = (int) Math.Ceiling(ceiling);

            var positiveCount = new int[segments];
            int startCases = 0;
            int endCases = segmentRange;

            for (int i = 0; i < positiveCount.Length; i++)
            {
                foreach (var currData in this.CovidRecords)
                {
                    if (startCases <= currData.PositiveCasesIncrease && currData.PositiveCasesIncrease <= endCases)
                    {
                        positiveCount[i]++;
                    }
                }

                startCases = endCases + 1;
                endCases = segmentRange * (i + 2);
            }

            return positiveCount;
        }

        #endregion
    }
}