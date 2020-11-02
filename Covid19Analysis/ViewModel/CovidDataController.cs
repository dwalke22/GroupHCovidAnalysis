using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        /// 
        /// </summary>
        public ObservableCollection<CovidData> CovidDatas
        {
            get { return covidDatas; }
            set { covidDatas = value; }

        }

        private CovidData selectedCovidData;
        
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public string[] States => StateEnum.StatesArray();

        private string selectedState;

        /// <summary>
        /// 
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
