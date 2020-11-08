using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     CovidRecords class that stores a collection of Covid data for a state
    /// </summary>
    public class CovidDataCollection : IList<CovidData>
    {
        #region Properties

        /// <summary>
        ///     The List of <see cref="CovidData" />
        /// </summary>
        private IList<CovidData> CovidRecords { get; }

        /// <summary>
        ///     The Count for the number of records in the collection
        /// </summary>
        public int Count => this.CovidRecords.Count;

        /// <summary>
        ///     The read only property
        /// </summary>
        public bool IsReadOnly => this.CovidRecords.IsReadOnly;

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CovidData this[int index]
        {
            get => this.CovidRecords[index];
            set => this.CovidRecords[index] = value;
        }

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
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(CovidData item)
        {
            return this.CovidRecords.Remove(item);
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            this.CovidRecords.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(CovidData item)
        {
            return this.CovidRecords.Contains(item);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CovidData[] array, int arrayIndex)
        {
            this.CovidRecords.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CovidData> GetEnumerator()
        {
            return this.CovidRecords.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(CovidData item)
        {
            return this.CovidRecords.IndexOf(item);
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, CovidData item)
        {
            this.CovidRecords.Insert(index, item);
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.CovidRecords.RemoveAt(index);
        }

        /// <summary>
        ///     Add all of the data to the collection
        /// </summary>
        /// <param name="data"></param>
        public void AddAll(List<CovidData> data)
        {
            foreach (var covidData in data)
            {
                this.CovidRecords.Add(covidData);
            }
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
            var firstPositiveCovidData = this.FindFirstPositiveTest();
            var averagePositive = this.CovidRecords.Where(covidData => covidData.Date >= firstPositiveCovidData.Date)
                                      .Average(covidData => covidData.PositiveCasesIncrease);
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
            double totalTest = this.CovidRecords.Sum(covidData => covidData.TotalTest);
            var totalPositives = this.CovidRecords.Sum(covidData => covidData.PositiveCasesIncrease);
            if (totalTest <= 0)
            {
                throw new ArithmeticException("Cannot divide by zero");
            }

            return totalPositives / totalTest;
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
            var firstPositiveCovidData = this.GetFirstPositiveCovidData();
            var numberOfDays = this.CovidRecords.Where(covidData =>
                                       covidData.PositiveCasesIncrease > 0 &&
                                       covidData.Date >= firstPositiveCovidData.Date)
                                   .Count(data => data.PositiveCasesIncrease < casesThreshold);

            return numberOfDays;
        }

        /// <summary>
        ///     Find the first date of a Positive Case
        ///     Precondition: this.count > 0
        ///     Post condition: None
        /// </summary>
        /// <returns>
        ///     Returns the <see cref="CovidData" /> of the first positive case
        /// </returns>
        private CovidData GetFirstPositiveCovidData()
        {
            this.CheckCollectionIsPopulated();
            var earliestDate = this.CovidRecords.Where(covidData => covidData.PositiveCasesIncrease > 0)
                                   .OrderBy(covidData => covidData.Date).First();
            return earliestDate;
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
        ///     Finds the positive cases between values.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>the number of positive cases between the bounds</returns>
        public int FindPositiveCasesBetweenValues(int lowerBound, int upperBound)
        {
            this.CheckCollectionIsPopulated();
            return this.CovidRecords.Count(currentCovidData =>
                currentCovidData.PositiveCasesIncrease >= lowerBound &&
                currentCovidData.PositiveCasesIncrease <= upperBound);
        }

        /// <summary>
        ///     Checks to see the the highest positive increase is in between the lowerBound and upperBound
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>true if the highest positive increase is between the bounds, false if else</returns>
        public bool BoundsContainHighestIncrease(int lowerBound, int upperBound)
        {
            this.CheckCollectionIsPopulated();
            var shouldHistogramStop = false;
            var highestPositiveTests = this.FindHighestNumberOfPositiveCasesInSingleDay().PositiveCasesIncrease;

            if (highestPositiveTests >= lowerBound && highestPositiveTests <= upperBound)
            {
                shouldHistogramStop = true;
            }

            return shouldHistogramStop;
        }

        /// <summary>
        ///     Finds the average of the current number of current hospitalizations
        /// </summary>
        /// <returns>The average number of current hospitalizations</returns>
        public double findAverageCurrentHospitalization()
        {
            this.CheckCollectionIsPopulated();
            return this.CovidRecords.Where(covidData => covidData.CurrentHospitalized >= 0)
                       .Average(covidData => covidData.CurrentHospitalized);
        }

        /// <summary>
        ///     Finds the coivdData with the highest number of current hospitalizations
        /// </summary>
        /// <returns>The coivd data with the highest number of current hospitalizations</returns>
        public CovidData findHighestCurrentHospitalization()
        {
            this.CheckCollectionIsPopulated();
            return this.CovidRecords.OrderByDescending(coivdData => coivdData.CurrentHospitalized).First();
        }

        /// <summary>
        ///     Finds the day with the Lowest Current Hospitalization number
        /// </summary>
        /// <returns>The CovidData with the lowest number of current hospitalization</returns>
        public CovidData findLowestCurrentHospitalization()
        {
            return this.CovidRecords.Where(coivdData => coivdData.CurrentHospitalized > 0)
                       .OrderBy(covidData => covidData.CurrentHospitalized).First();
        }

        /// <summary>
        ///     Replaces the covid data if the date and state already exist in the collection.
        /// </summary>
        /// <param name="covidData">The covid data.</param>
        /// <exception cref="ArgumentNullException">if covidData is null</exception>
        public void ReplaceCovidData(CovidData covidData)
        {
            if (covidData == null)
            {
                throw new ArgumentNullException();
            }

            if (this.CovidRecords.Count == 0)
            {
                throw new Exception();
            }

            for (var i = this.CovidRecords.Count - 1; i >= 0; i--)
            {
                this.compareThenRemove(covidData, i);
            }
        }

        private void compareThenRemove(CovidData covidData, int iterationIndex)
        {
            if (!compareCovidDataStateAndDate(covidData, this.CovidRecords[iterationIndex]))
            {
                return;
            }

            var index = this.CovidRecords.IndexOf(this.CovidRecords[iterationIndex]);

            this.CovidRecords[index] = covidData;
        }

        /// <summary>
        ///     Compares the covid data state and date.
        /// </summary>
        /// <param name="firstCovidData">The first covid data.</param>
        /// <param name="secondCovidData">The second covid data.</param>
        /// <returns>True if the date and state are the same, false otherwise</returns>
        private static bool compareCovidDataStateAndDate(CovidData firstCovidData, CovidData secondCovidData)
        {
            return secondCovidData.Date.Equals(firstCovidData.Date) &&
                   secondCovidData.State.Equals(firstCovidData.State);
        }

        #endregion
    }
}