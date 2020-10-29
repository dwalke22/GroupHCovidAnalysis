using System;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     CovidRecords class that holds data on Covid CovidRecords for a given date.
    /// </summary>
    public class CovidData
    {
        #region Properties

        /// <summary>
        ///     Gets the date
        /// </summary>
        /// <value>
        ///     The Date
        /// </value>
        public DateTime Date { get; }

        /// <summary>
        ///     Gets the state
        /// </summary>
        /// <value>
        ///     The state
        /// </value>
        public string State { get; }

        /// <summary>
        ///     Gets the number of positive cases for the date
        /// </summary>
        /// <value>
        ///     The number of positive cases
        /// </value>
        public int PositiveCasesIncrease { get; set; }

        /// <summary>
        ///     Gets the number of negative cases
        /// </summary>
        /// <value>
        ///     The number of negative cases
        /// </value>
        public int NegativeCasesIncrease { get; set; }

        /// <summary>
        ///     Gets the number of deaths for the date
        /// </summary>
        /// <value>
        ///     The number of deaths
        /// </value>
        public int DeathNumbers { get; set; }

        /// <summary>
        ///     Gets the number of hospitalized cases
        /// </summary>
        /// <value>
        ///     The number of hospitalized cases
        /// </value>
        public int HospitalizedNumbers { get; set; }

        /// <summary>
        ///     Gets the total number of tests
        /// </summary>
        /// <value>
        ///     The total number of cases
        /// </value>
        public int TotalTest => this.PositiveCasesIncrease + this.NegativeCasesIncrease;

        /// <summary>
        ///     The Overall positive percentage for the <see cref="CovidData" />
        /// </summary>
        /// <value>
        ///     The <see cref="double" /> of the overall percentage
        /// </value>
        public double OverallPositivePercentage
        {
            get
            {
                if (this.TotalTest == 0)
                {
                    return Convert.ToDouble(this.PositiveCasesIncrease);
                }

                return Convert.ToDouble(this.PositiveCasesIncrease) / this.TotalTest;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CovidData" /> class
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="state">The last name.</param>
        /// <param name="positiveCasesIncrease">The number of positive cases for the date.</param>
        /// <param name="negativeCasesIncrease">The number of negative cases for the date.</param>
        /// <param name="deathNumbers">The number of deaths for the date.</param>
        /// <param name="hospitalizedNumbers">The number of hospitalized cases.</param>
        /// <exception cref="NullReferenceException">state</exception>
        public CovidData(DateTime date, string state, int positiveCasesIncrease, int negativeCasesIncrease,
            int deathNumbers, int hospitalizedNumbers)
        {
            this.Date = date;
            this.State = state ?? throw new ArgumentNullException(nameof(state));
            this.PositiveCasesIncrease = positiveCasesIncrease;
            this.NegativeCasesIncrease = negativeCasesIncrease;
            this.DeathNumbers = deathNumbers;
            this.HospitalizedNumbers = hospitalizedNumbers;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Converts to a string for file writing
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance object
        /// </returns>
        public override string ToString()
        {
            return
                $"{this.Date:yyyyMMdd},{this.State},{this.PositiveCasesIncrease},{this.NegativeCasesIncrease}," +
                $"{this.DeathNumbers},{this.HospitalizedNumbers}";
        }

        #endregion
    }
}