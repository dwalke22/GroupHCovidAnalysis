using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Covid19Analysis.Annotations;
using Covid19Analysis.DataHandling;
using Covid19Analysis.EnumTypes;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Utility;

namespace Covid19Analysis.ViewModel
{
    class CovidDataController : INotifyPropertyChanged
    {
        private const int DefaultUpperLimit = 2500;
        private const int DefaultLowerLimit = 1000;
        private const int DefaultBinSize = 500;

        private CovidDataCreator dataCreator;
        private CovidDataCollection covidDataCollection;

        /// <summary>
        ///     The Remove Command
        /// </summary>
        public RelayCommand ClearCommand { get; set; }

        /// <summary>
        ///     The Change State Command
        /// </summary>
        public RelayCommand ChangeStateCommand { get; set; }

        /// <summary>
        /// The Load Data Command
        /// </summary>
        public RelayCommand LoadData { get; set; }

        private ObservableCollection<CovidData> selectedStateData;
        
        /// <summary>
        ///     The Collection to be used in the 
        /// </summary>
        public ObservableCollection<CovidData> SelectedStateData
        {
            get { return this.selectedStateData; }
            set
            {
                this.selectedStateData = value;
                this.OnPropertyChanged();
                this.ClearCommand.OnCanExecuteChanged();
            }

        }

        private CovidData selectedCovidData;
        
        /// <summary>
        ///     The Selected CovidDate from the List
        /// </summary>
        public CovidData SelectedCovidData
        {
            get { return selectedCovidData; }
            set
            {
                selectedCovidData = value;
                this.OnPropertyChanged();
            }
        }
        
        /// <summary>
        ///     The States to be used in the ComboBox
        /// </summary>
        public string[] States => StateEnum.StatesArray();

        private string selectedState;

        /// <summary>
        ///     The Selected State to get data on
        /// </summary>
        public string SelectedState
        {
            get { return this.selectedState; }
            set
            {
                this.selectedState = value;
                this.OnPropertyChanged();
                this.ChangeStateCommand.OnCanExecuteChanged();
            }
        }

        private int binSize;

        /// <summary>
        ///     The Bin Size to be used for output
        /// </summary>
        public int BinSize
        {
            get { return binSize; }
            set
            {
                binSize = value;
                this.OnPropertyChanged();
            }
        }

        private int lowerBoundary;

        /// <summary>
        ///     The LowerBoundary to be used in analysis
        /// </summary>
        public int LowerBoundary
        {
            get { return lowerBoundary; }
            set
            {
                lowerBoundary = value;
                this.OnPropertyChanged();
            }
        }

        private int upperBoundary;

        /// <summary>
        ///     The Upper Boundary to be used in the summary output
        /// </summary>
        public int UpperBoundary
        {
            get { return upperBoundary; }
            set
            {
                upperBoundary = value;
                this.OnPropertyChanged();
            }
        }

        private string summaryOutput;

        /// <summary>
        ///     The summary output
        /// </summary>
        public string SummaryOutput
        {
            get { return summaryOutput; }
            set
            {
                summaryOutput = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Add relay command
        /// </summary>
        public RelayCommand AddCommand;


        private DateTime addDateTime;

        /// <summary>
        ///     The DateTime To Be Added
        /// </summary>
        public DateTime AddDateTime
        {
            get { return addDateTime; }
            set
            {
                addDateTime = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private string addState;

        /// <summary>
        ///     The State of the added Data
        /// </summary>
        public string AddState
        {
            get { return addState; }
            set
            {
                addState = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private int positiveCases;

        /// <summary>
        ///     The Number of positive cases of the date
        /// </summary>
        public int PositiveCases
        {
            get { return positiveCases; }
            set
            {
                positiveCases = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private int negativeCases;

        /// <summary>
        ///     The number of negative cases for the day
        /// </summary>
        public int NegativeCases
        {
            get { return negativeCases; }
            set
            {
                negativeCases = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private int currentHospitalized;

        /// <summary>
        ///     The number of currently hospitalized
        /// </summary>
        public int CurrentHospitalized
        {
            get { return currentHospitalized; }
            set
            {
                currentHospitalized = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private int deaths;

        /// <summary>
        ///     The Number of deaths in the given day
        /// </summary>
        public int Deaths
        {
            get { return deaths; }
            set
            {
                deaths = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private int hospitalized;

        /// <summary>
        ///     the number of hospitalizations on the day
        /// </summary>
        public int Hospitalized
        {
            get { return hospitalized; }
            set
            {
                hospitalized = value;
                this.OnPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CovidDataController()
        {
            this.dataCreator = new CovidDataCreator();
            this.covidDataCollection = new CovidDataCollection();
            this.selectedStateData = this.covidDataCollection.ToObservableCollection();
            this.setDefaultValues();
            this.loadCommands();
        }

        private void setDefaultValues()
        {
            this.upperBoundary = DefaultUpperLimit;
            this.lowerBoundary = DefaultLowerLimit;
            this.binSize = DefaultBinSize;
        }

        private void loadCommands()
        {
            this.ClearCommand = new RelayCommand(ClearCollection, CanClearCollection);
            this.ChangeStateCommand = new RelayCommand(ChangeState, CanChangeState);
            this.LoadData = new RelayCommand(ProcessData, CanLoad);
            this.AddCommand = new RelayCommand(AddData, CanAdd);
        }

        private bool CanAdd(object obj)
        {
            if (this.AddDateTime == null || this.AddState == null)
            {
                return false;
            }
            else
            {
                return this.AddState.Length > 0 && this.PositiveCases > 0 &&
                       this.Hospitalized > 0 && this.Deaths > 0 && this.CurrentHospitalized > 0;
            }
        }

        private void AddData(object obj)
        {
            var covidData = new CovidData { Date = this.AddDateTime.Date, State = this.AddState, PositiveCasesIncrease = this.PositiveCases, NegativeCasesIncrease = this.NegativeCases, CurrentHospitalized = this.CurrentHospitalized, DeathNumbers = this.Deaths, HospitalizedNumbers = this.Hospitalized };
        }

        private bool CanLoad(object obj)
        {
            return true;
        }

        private async void ProcessData(object obj)
        {

        }

        private bool CanChangeState(object obj)
        {
            return this.dataCreator.CovidData.Count > 0;
        }

        private void ChangeState(object obj)
        {
            this.SelectedStateData = this.dataCreator.GetStateCovidData(this.selectedState).ToObservableCollection();
        }

        private void ClearCollection(object obj)
        {
            this.SelectedStateData.Clear();
            this.summaryOutput = string.Empty;
        }

        private bool CanClearCollection(object obj)
        {
            return this.SelectedStateData.Count > 0;
        }

        private void GenerateNewSummaryOutput()
        {
            var dataFormatter = new CovidDataFormatter(this.covidDataCollection);
            var monthlyData = new MonthlyCovidDataCollection(this.covidDataCollection);
            this.SummaryOutput = dataFormatter.FormatGeneralData(this.upperBoundary, this.lowerBoundary, this.binSize) +
                                 $"{Environment.NewLine}" +
                                 dataFormatter.FormatMonthlyData(monthlyData);
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        }
}
