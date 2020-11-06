using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Covid19Analysis.Annotations;
using Covid19Analysis.DataHandling;
using Covid19Analysis.EnumTypes;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Utility;

namespace Covid19Analysis.ViewModel
{
    public class CovidDataController : INotifyPropertyChanged
    {

        private CovidDataCreator dataCreator;
        private CovidDataCollection covidDataCollection;

        /// <summary>
        ///     The Remove Data Command
        /// </summary>
        public RelayCommand RemoveCommand { get; set; }

        private ObservableCollection<CovidData> observableCovidCollection;
        
        /// <summary>
        ///     The Collection to be used in the 
        /// </summary>
        public ObservableCollection<CovidData> ObservableCovidCollection
        {
            get { return this.observableCovidCollection; }
            set
            {
                this.observableCovidCollection = value;
                this.OnPropertyChanged();
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
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public CovidDataController()
        {
            this.dataCreator = new CovidDataCreator();
            this.covidDataCollection = new CovidDataCollection();
            this.toObservableCollection();
            this.loadCommands();
        }

        private void loadCommands()
        {
            this.RemoveCommand = new RelayCommand(DeleteData, CanDeletData);
        }

        private bool CanDeletData(object obj)
        {
            return this.SelectedCovidData != null;
        }

        private void DeleteData(object obj)
        {
            this.covidDataCollection.Remove(this.selectedCovidData);
            this.toObservableCollection();
        }

        
        private void toObservableCollection()
        {
            this.ObservableCovidCollection = this.covidDataCollection.ToObservableCollection();
            
        }

        public void setObservableCollection(CovidDataCollection collection)
        {
            this.covidDataCollection = collection;
            this.toObservableCollection();
        }

        public void toCollection()
        {
            this.covidDataCollection.Clear();
            this.covidDataCollection.AddAll(this.ObservableCovidCollection.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        }
}
