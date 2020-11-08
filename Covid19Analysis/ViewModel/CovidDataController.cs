using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Covid19Analysis.DataHandling;
using Covid19Analysis.EnumTypes;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Properties;
using Covid19Analysis.Utility;

namespace Covid19Analysis.ViewModel
{
    /// <summary>
    ///     The CovidDataController class
    /// </summary>
    public class CovidDataController : INotifyPropertyChanged
    {
        #region Data members

        private CovidDataCollection covidDataCollection;

        private ObservableCollection<CovidData> observableCovidCollection;

        private string positiveIncreasesText;

        private string negativeIncreasesText;

        private string currentHospitalizedText;

        private string hospitalizedText;

        private string deathsText;

        private CovidData selectedCovidData;

        private string selectedState;

        #endregion

        #region Properties

        /// <summary>
        ///     the DataCreator property
        /// </summary>
        public CovidDataCreator DataCreator { get; }

        /// <summary>
        ///     The Remove Data Command
        /// </summary>
        public RelayCommand RemoveCommand { get; set; }

        /// <summary>
        ///     The Update Command
        /// </summary>
        public RelayCommand UpdateCommand { get; set; }

        /// <summary>
        ///     The Collection to be used in the
        /// </summary>
        public ObservableCollection<CovidData> ObservableCovidCollection
        {
            get => this.observableCovidCollection;
            set
            {
                this.observableCovidCollection = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Positive IncreaseText
        /// </summary>
        public string PositiveIncreasesText
        {
            get => this.positiveIncreasesText;
            set
            {
                this.positiveIncreasesText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     the Negative Increase Text property
        /// </summary>
        public string NegativeIncreasesText
        {
            get => this.negativeIncreasesText;
            set
            {
                this.negativeIncreasesText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Current Hospitalized Text Property
        /// </summary>
        public string CurrentHospitalizedText
        {
            get => this.currentHospitalizedText;
            set
            {
                this.currentHospitalizedText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Hospitalized Text property
        /// </summary>
        public string HospitalizedText
        {
            get => this.hospitalizedText;
            set
            {
                this.hospitalizedText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     the Death Text Property
        /// </summary>
        public string DeathsText
        {
            get => this.deathsText;
            set
            {
                this.deathsText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Selected CovidDate from the List
        /// </summary>
        public CovidData SelectedCovidData
        {
            get => this.selectedCovidData;
            set
            {
                this.selectedCovidData = value;
                this.OnPropertyChanged();
                this.RemoveCommand.OnCanExecuteChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The States to be used in the ComboBox
        /// </summary>
        public string[] States => StateEnum.StatesArray();

        /// <summary>
        ///     The Selected State to get data on
        /// </summary>
        public string SelectedState
        {
            get => this.selectedState;
            set
            {
                this.selectedState = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// </summary>
        public CovidDataController()
        {
            this.DataCreator = new CovidDataCreator();
            this.covidDataCollection = new CovidDataCollection();
            this.toObservableCollection();
            this.loadCommands();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void loadCommands()
        {
            this.RemoveCommand = new RelayCommand(this.deleteData, this.canDeleteData);
            this.UpdateCommand = new RelayCommand(this.updateProperties, this.shouldDisable);
        }

        private bool shouldDisable(object obj)
        {
            return this.selectedCovidData != null;
        }

        private void updateProperties(object obj)
        {
            this.PositiveIncreasesText = this.selectedCovidData.PositiveCasesIncrease.ToString();
            this.NegativeIncreasesText = this.selectedCovidData.NegativeCasesIncrease.ToString();
            this.CurrentHospitalizedText = this.selectedCovidData.CurrentHospitalized.ToString();
            this.HospitalizedText = this.selectedCovidData.HospitalizedNumbers.ToString();
            this.DeathsText = this.selectedCovidData.DeathNumbers.ToString();
        }

        private bool canDeleteData(object obj)
        {
            return this.SelectedCovidData != null;
        }

        private void deleteData(object obj)
        {
            this.covidDataCollection.Remove(this.selectedCovidData);
            this.toObservableCollection();
        }

        /// <summary>
        ///     Converts a collection to an observable collection
        /// </summary>
        public void toObservableCollection()
        {
            this.ObservableCovidCollection = this.covidDataCollection.ToObservableCollection();
        }

        /// <summary>
        ///     Sets an observable collection
        /// </summary>
        /// <param name="collection"></param>
        public void setObservableCollection(CovidDataCollection collection)
        {
            this.covidDataCollection = collection;
            this.toObservableCollection();
        }

        /// <summary>
        /// </summary>
        public void handleSelectionUpdate()
        {
            this.covidDataCollection.ReplaceCovidData(this.selectedCovidData);
            this.toObservableCollection();
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}