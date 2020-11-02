using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Covid19Analysis.Annotations;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;

namespace Covid19Analysis.ViewModel
{
    class CovidDataController : INotifyPropertyChanged

    {
    private CovidDataCollection covidDataCollection;

    private ObservableCollection<CovidData> covidDatas;

    public ObservableCollection<CovidData> CovidDatas
    {
        get { return covidDatas; }
        set { covidDatas = value; }

    }

    private CovidData selectedCovidData;

    public CovidData SelectedCovidData
    {
        get { return selectedCovidData; }
        set
        {
            selectedCovidData = value;
            this.OnPropertyChanged();
        }
    }

    public CovidDataController()
    {
        this.covidDataCollection = new CovidDataCollection();

        this.covidDatas = this.covidDataCollection.ToObservableCollection();
    }

    public int twoPlusTwo()
    {
        return 2 + 2;
    }



    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    }
}
