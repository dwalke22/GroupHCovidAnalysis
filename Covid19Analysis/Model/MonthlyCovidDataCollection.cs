using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     The Monthly Covid Data Collection
    /// </summary>
    public class MonthlyCovidDataCollection
    {
        #region Properties

        /// <summary>
        ///     The Year of the Monthly Collection
        /// </summary>
        public int Year { get; }

        /// <summary>
        ///     The monthly break down of the covid data
        /// </summary>
        public CovidDataCollection[] MonthlyCollection { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="MonthlyCovidDataCollection" /> object
        /// </summary>
        /// <param name="covidDataCollection"> the Data Collection to separate</param>
        public MonthlyCovidDataCollection(CovidDataCollection covidDataCollection)
        {
            this.Year = covidDataCollection.FindFirstPositiveTest().Year;
            this.MonthlyCollection = this.GetMonthlyCovidData(covidDataCollection);
        }

        #endregion

        #region Methods

        private CovidDataCollection[] GetMonthlyCovidData(CovidDataCollection monthlyCollection)
        {
            var monthlyData = new CovidDataCollection[12];
            this.instanciateNewMonthlyData(monthlyData);
            this.seperateDataIntoMonths(monthlyData, monthlyCollection);
            return monthlyData;
        }

        private void instanciateNewMonthlyData(CovidDataCollection[] monthlyData)
        {
            for (var i = 0; i < monthlyData.Length; i++)
            {
                monthlyData[i] = new CovidDataCollection();
            }
        }

        private void seperateDataIntoMonths(CovidDataCollection[] monthlyData, CovidDataCollection covidData)
        {
            var monthlyGroups = from data in covidData group data by data.Date.Month;
            foreach (var monthlyGroup in monthlyGroups)
            {
                var monthKey = monthlyGroup.Key;
                foreach (var data in monthlyGroup)
                {
                    monthlyData[monthKey - 1].Add(data);
                }
            }
        }

        #endregion
    }
}