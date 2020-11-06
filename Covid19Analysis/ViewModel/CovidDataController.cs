using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Covid19Analysis.Annotations;
using Covid19Analysis.DataHandling;
using Covid19Analysis.EnumTypes;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;

namespace Covid19Analysis.ViewModel
{
    /// <summary>
    ///     The Controller class
    /// </summary>
    public class CovidDataController : INotifyPropertyChanged
    {

        private CovidDataCreator dataCreator;
        private CovidDataCollection covidDataCollection;


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
            this.selectedStateData = this.covidDataCollection.ToObservableCollection();
        }


        /// <summary>
        ///     The PropertyChanged event handler
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
