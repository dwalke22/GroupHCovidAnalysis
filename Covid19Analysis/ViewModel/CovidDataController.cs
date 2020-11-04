using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Covid19Analysis.Annotations;
using Covid19Analysis.EnumTypes;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;

namespace Covid19Analysis.ViewModel
{
    class CovidDataController : INotifyPropertyChanged

    {
        private CovidDataCollection covidDataCollection;

        private ObservableCollection<CovidData> covidDatas;
        
        /// <summary>
        ///     The Collection to be used in the 
        /// </summary>
        public ObservableCollection<CovidData> CovidDatas
        {
            get { return covidDatas; }
            set
            {
                this.covidDatas = value;
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


        /// <summary>
        /// 
        /// </summary>
        public CovidDataController()
        {
            this.covidDataCollection = new CovidDataCollection();

            this.covidDatas = this.covidDataCollection.ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        }
}
