using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Properties;
using Covid19Analysis.Utility;

namespace Covid19Analysis.ViewModel
{
    /// <summary>
    ///     Works between the model and the viewmodel for use in databinding
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class CovidDataController : INotifyPropertyChanged
    {
        private CovidDataCollection covidDataCollection;

        private string currentHospitalizedText;


        private string deathsText;

        private string hospitalizedText;

        private string negativeIncreasesText;

        private ObservableCollection<CovidData> observableCovidCollection;

        private string positiveIncreasesText;

        private CovidData selectedCovidData;

        private string selectedState;


        /// <summary>
        /// </summary>
        public CovidDataController()
        {
            covidDataCollection = new CovidDataCollection();
            ToObservableCollection();
            LoadCommands();
        }

        /// <summary>
        ///     The Remove Data Command
        /// </summary>
        public RelayCommand RemoveCommand { get; set; }

        /// <summary>
        ///     Gets or sets the enable command.
        /// </summary>
        /// <value>
        ///     The enable command.
        /// </value>
        public RelayCommand EnableCommand { get; set; }

        /// <summary>
        ///     The Collection to be used in the
        /// </summary>
        public ObservableCollection<CovidData> ObservableCovidCollection
        {
            get => observableCovidCollection;
            set
            {
                observableCovidCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the positive increases text.
        /// </summary>
        /// <value>
        ///     The positive increases text.
        /// </value>
        public string PositiveIncreasesText
        {
            get => positiveIncreasesText;
            set
            {
                positiveIncreasesText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the negative increases text.
        /// </summary>
        /// <value>
        ///     The negative increases text.
        /// </value>
        public string NegativeIncreasesText
        {
            get => negativeIncreasesText;
            set
            {
                negativeIncreasesText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the current hospitalized text.
        /// </summary>
        /// <value>
        ///     The current hospitalized text.
        /// </value>
        public string CurrentHospitalizedText
        {
            get => currentHospitalizedText;
            set
            {
                currentHospitalizedText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the hospitalized text.
        /// </summary>
        /// <value>
        ///     The hospitalized text.
        /// </value>
        public string HospitalizedText
        {
            get => hospitalizedText;
            set
            {
                hospitalizedText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the deaths text.
        /// </summary>
        /// <value>
        ///     The deaths text.
        /// </value>
        public string DeathsText
        {
            get => deathsText;
            set
            {
                deathsText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The Selected CovidDate from the List
        /// </summary>
        public CovidData SelectedCovidData
        {
            get => selectedCovidData;
            set
            {
                selectedCovidData = value;
                OnPropertyChanged();
                RemoveCommand.OnCanExecuteChanged();
                EnableCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     The States to be used in the ComboBox
        /// </summary>
        public string[] States => StateResources.States.StatesArray();

        /// <summary>
        ///     The Selected State to get data on
        /// </summary>
        public string SelectedState
        {
            get => selectedState;
            set
            {
                selectedState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadCommands()
        {
            RemoveCommand = new RelayCommand(DeleteData, CanDeleteData);
            EnableCommand = new RelayCommand(UpdateProperties, ShouldDisable);
        }

        private bool ShouldDisable(object obj)
        {
            return selectedCovidData != null;
        }

        private void UpdateProperties(object obj)
        {
            PositiveIncreasesText = selectedCovidData.PositiveCasesIncrease.ToString();
            NegativeIncreasesText = selectedCovidData.NegativeCasesIncrease.ToString();
            CurrentHospitalizedText = selectedCovidData.CurrentHospitalized.ToString();
            HospitalizedText = selectedCovidData.HospitalizedNumbers.ToString();
            DeathsText = selectedCovidData.DeathNumbers.ToString();
        }

        private bool CanDeleteData(object obj)
        {
            return SelectedCovidData != null;
        }

        private void DeleteData(object obj)
        {
            covidDataCollection.Remove(selectedCovidData);
            ToObservableCollection();
        }


        /// <summary>
        ///     turns the collection into an observable collection
        /// </summary>
        public void ToObservableCollection()
        {
            ObservableCovidCollection = covidDataCollection.ToObservableCollection();
        }

        /// <summary>
        ///     Sets the observable collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void SetObservableCollection(CovidDataCollection collection)
        {
            covidDataCollection = collection;
            ToObservableCollection();
        }

        /// <summary>
        ///     adds the contents of the observable collection to the normal collection
        /// </summary>
        public void ToCollection()
        {
            covidDataCollection.Clear();
            covidDataCollection.AddAll(ObservableCovidCollection.ToList());
        }

        /// <summary>
        ///     Handles the selection update.
        /// </summary>
        public void HandleSelectionUpdate()
        {
            covidDataCollection.ReplaceCovidData(selectedCovidData);
            ToObservableCollection();
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}