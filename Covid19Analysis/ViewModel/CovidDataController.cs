using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        /// 
        /// </summary>
        public CovidDataController()
        {
            this.dataCreator = new CovidDataCreator();
            this.covidDataCollection = new CovidDataCollection();
            this.selectedStateData = this.covidDataCollection.ToObservableCollection();
            this.loadCommands();
        }

        private void loadCommands()
        {
            this.ClearCommand = new RelayCommand(ClearCollection, CanClearCollection);
            this.ChangeStateCommand = new RelayCommand(ChangeState, CanChangeState);
        }

        private bool CanChangeState(object obj)
        {
            return this.dataCreator.CovidData.Count > 0;
        }

        private void ChangeState(object obj)
        {
            this.selectedStateData = this.dataCreator.GetStateCovidData(this.selectedState).ToObservableCollection();
        }

        private void ClearCollection(object obj)
        {
            this.selectedStateData.Clear();
            this.summaryOutput = string.Empty;
        }

        private bool CanClearCollection(object obj)
        {
            return this.selectedStateData.Count > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        }
}
