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
        ///     data != null
        ///     PostCondition:
        ///     Count = Count@prev + 1
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
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     Returns the <see cref="DateTime" /> of the first positive case
        /// </returns>
        public DateTime FindFirstPositiveTest()
        {
            this.CheckCollectionIsPopulated();
            var earliestDate = this.CovidRecords.Where(covidData => covidData.PositiveCasesIncrease > 0)
                                   .OrderBy(covidData => covidData.Date).First().Date;
            return earliestDate;
        }

        private void CheckCollectionIsPopulated()
        {
            if (this.Count <= 0)
            {
                throw new InvalidOperationException("Collection contains no elements");
            }
        }

        /// <summary>
        ///     Finds and returns the record for most positive cases
        ///     in a single day
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> record for most positive cases
        ///     in a single day
        /// </returns>
        public CovidData FindHighestNumberOfPositiveCasesInSingleDay()
        {
            this.CheckCollectionIsPopulated();
            var highestPositive = this.CovidRecords.Where(covidData => covidData.OverallPositivePercentage > 0)
                                      .OrderByDescending(covidData => covidData.PositiveCasesIncrease).First();
            return highestPositive;
        }

        /// <summary>
        ///     Finds and returns data on the day with the most
        ///     negative cases in a single day
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> on the most negative cases
        ///     in a single day
        /// </returns>
        public CovidData FindHighestNumberOfNegativeCasesInSingleDay()
        {
            this.CheckCollectionIsPopulated();
            var highestNegatives = this.CovidRecords.OrderByDescending(covidData => covidData.NegativeCasesIncrease)
                                       .First();
            return highestNegatives;
        }

        /// <summary>
        ///     Finds the CovidData for the day with the most amount of tests
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the highest number of tests for a single day
        /// </returns>
        public CovidData FindHighestNumberOfTests()
        {
            this.CheckCollectionIsPopulated();
            var highestTest = this.CovidRecords.OrderByDescending(covidData => covidData.TotalTest).First();
            return highestTest;
        }

        /// <summary>
        ///     Finds the day with the highest number of deaths
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> for the day with the highest number of deaths
        /// </returns>
        public CovidData FindHighestNumberOfDeaths()
        {
            this.CheckCollectionIsPopulated();
            var highestDeaths = this.CovidRecords.OrderByDescending(covidData => covidData.DeathNumbers).First();
            return highestDeaths;
        }

        /// <summary>
        ///     Finds the highest number of hospitalizations for a single day
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     The <see cref="CovidData" /> with the highest number of hospitalizations
        /// </returns>
        public CovidData FindHighestNumberOfHospitalizations()
        {
            this.CheckCollectionIsPopulated();
            var highestHospitalized =
                this.CovidRecords.OrderByDescending(covidData => covidData.HospitalizedNumbers).First();
            return highestHospitalized;
        }

        /// <summary>
        ///     Finds the day with the highest positive percentage
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the highest percentage with
        ///     a positive cases
        /// </returns>
        public CovidData FindHighestPositivePercentage()
        {
            this.CheckCollectionIsPopulated();
            var highestPositive = this.CovidRecords.OrderByDescending(covidData => covidData.OverallPositivePercentage)
                                      .First();
            return highestPositive;
        }

        /// <summary>
        ///     Finds the average number of positive test per day since first positive
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     The average <see cref="double" /> of positive test per day
        /// </returns>
        public double FindAveragePositiveCasesSinceFirstPositive()
        {
            this.CheckCollectionIsPopulated();
            var averagePositive = this.CovidRecords.Average(covidData => covidData.PositiveCasesIncrease);
            return averagePositive;
        }

        /// <summary>
        ///     Finds the overall rate of positive cases
        ///     Precondition: this.Count > 0
        ///     PostCondition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Double" /> that represents the percentage of positive cases
        /// </returns>
        public double FindOverallPositivityRate()
        {
            this.CheckCollectionIsPopulated();
            var averageRate = this.CovidRecords.Average(covidData => covidData.OverallPositivePercentage);
            return averageRate;
        }

        /// <summary>
        ///     Finds the number of days with more positive than threshold
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <param name="casesThreshold">
        ///     The threshold to check for days this more than threshold positives
        /// </param>
        /// <returns>
        ///     The number <see cref="int" /> of days with more than 2500 positives
        /// </returns>
        public int FindNumberOfDaysWithCasesOverThreshold(int casesThreshold)
        {
            this.CheckCollectionIsPopulated();
            return this.CovidRecords.Count(data => data.PositiveCasesIncrease > casesThreshold);
        }

        /// <summary>
        ///     Finds the number of days with less than cases threshold
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <param name="casesThreshold">
        ///     The threshold to check for days with less than threshold positives
        /// </param>
        /// <returns>
        ///     The number <see cref="int" /> of days with less than 1000 positives
        /// </returns>
        public int FindNumberOfDaysWithCasesUnderLowerThreshold(int casesThreshold)
        {
            this.CheckCollectionIsPopulated();
            var date = this.FindFirstPositiveTest();
            return this.CovidRecords.Where(covidData => covidData.PositiveCasesIncrease > 0 && covidData.Date > date)
                       .Count(data => data.PositiveCasesIncrease < casesThreshold);
        }

        /// <summary>
        ///     Finds the day with the lowest number of Positive Cases
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> with the lowest number of positive cases
        /// </returns>
        public CovidData FindLowestPositiveCases()
        {
            this.CheckCollectionIsPopulated();
            var lowestPositive = this.CovidRecords.OrderBy(covidData => covidData.PositiveCasesIncrease).First();
            return lowestPositive;
        }

        /// <summary>
        ///     Finds the lowest number of total cases
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="CovidData" /> that had the lowest number of total cases
        /// </returns>
        public CovidData FindLowestTotalCases()
        {
            this.CheckCollectionIsPopulated();
            var lowestTotalCases = this.CovidRecords.OrderBy(covidData => covidData.TotalTest).First();
            return lowestTotalCases;
        }

        /// <summary>
        ///     Finds the number of average tests per day
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Double" /> that represents the average number of tests per day
        /// </returns>
        public double FindAverageNumberOfTestPerDay()
        {
            this.CheckCollectionIsPopulated();
            var averageTotalTest = this.CovidRecords.Where(covidData => covidData.TotalTest > 0)
                                       .Average(covidData => covidData.TotalTest);
            return averageTotalTest;
        }

        /// <summary>
        ///     Separates the <see cref="CovidDataCollection" /> into segments based on Positive cases
        ///     Precondition: this.count > 0 and segmentRange >= 1
        ///     Post condition: None
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

            this.CheckCollectionIsPopulated();
            var maxPositive = this.FindHighestNumberOfPositiveCasesInSingleDay().PositiveCasesIncrease;
            var ceiling = (double) maxPositive / segmentRange;
            var segments = (int) Math.Ceiling(ceiling);

            var positiveCount = new int[segments];
            var startCases = 0;
            var endCases = segmentRange;

            for (var i = 0; i < positiveCount.Length; i++)
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