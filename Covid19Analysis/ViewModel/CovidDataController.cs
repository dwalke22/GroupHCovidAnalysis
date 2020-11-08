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

        private CovidData selectedCovidData;

        private string selectedState;

        private int positiveCaseIncrease;

        private int negativeCaseIncrease;

        private int currentHospitalized;

        private int deaths;

        private int hospitalized;

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
        ///     The Selected CovidDate from the List
        /// </summary>
        public CovidData SelectedCovidData
        {
            get => this.selectedCovidData;
            set
            {
                this.selectedCovidData = value;
                this.PositiveCaseIncrease = this.selectedCovidData.PositiveCasesIncrease;
                this.NegativeCaseIncrease = this.selectedCovidData.NegativeCasesIncrease;
                this.CurrentHopitalized = this.selectedCovidData.CurrentHospitalized;
                this.Deaths = this.selectedCovidData.DeathNumbers;
                this.Hospitalized = this.selectedCovidData.HospitalizedNumbers;
                this.OnPropertyChanged();
                this.RemoveCommand.OnCanExecuteChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The number of positive cases
        /// </summary>
        public int PositiveCaseIncrease
        {
            get => this.positiveCaseIncrease;
            set
            {
                this.positiveCaseIncrease = value;
                this.OnPropertyChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The number of Negative cases
        /// </summary>
        public int NegativeCaseIncrease
        {
            get => this.negativeCaseIncrease;
            set
            {
                this.negativeCaseIncrease = value;
                this.OnPropertyChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The number of currently hospitalized
        /// </summary>
        public int CurrentHopitalized
        {
            get => this.currentHospitalized;
            set
            {
                this.currentHospitalized = value;
                this.OnPropertyChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     the number of deaths
        /// </summary>
        public int Deaths
        {
            get => this.deaths;
            set
            {
                this.deaths = value;
                this.OnPropertyChanged();
                this.UpdateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The number of hospitalized
        /// </summary>
        public int Hospitalized
        {
            get => this.hospitalized;
            set
            {
                this.hospitalized = value;
                this.OnPropertyChanged();
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
            this.UpdateCommand = new RelayCommand(this.updateProperties, this.canUpdate);
        }

        private bool canUpdate(object obj)
        {
            if (this.SelectedCovidData == null)
            {
                return false;
            }

            return this.PositiveCaseIncrease >= 0 && this.NegativeCaseIncrease >= 0 && this.CurrentHopitalized >= 0
                   && this.Deaths >= 0 && this.Hospitalized >= 0;
        }

        private void updateProperties(object obj)
        {
            this.SelectedCovidData.PositiveCasesIncrease = this.PositiveCaseIncrease;
            this.SelectedCovidData.NegativeCasesIncrease = this.NegativeCaseIncrease;
            this.SelectedCovidData.CurrentHospitalized = this.CurrentHopitalized;
            this.SelectedCovidData.DeathNumbers = this.Deaths;
            this.SelectedCovidData.HospitalizedNumbers = this.Hospitalized;
            this.covidDataCollection.ReplaceCovidData(this.SelectedCovidData);
            this.toObservableCollection();
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
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}