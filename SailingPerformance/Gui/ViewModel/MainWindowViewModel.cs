using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Eto.Drawing;
using Gui.Common;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;
using Microsoft.Win32;
using Dal.Repositories;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private DateTime _endDate;
        private DateTime _startDate;
        private int _selectedIndexBoat;
        private int _selectedIndexSession;
        private string _dataOrigin;

        public bool isDataComplete { get; set; }



        public ChartViewModel ChartViewModel { get; set; }

        public ObservableCollection<BoatDto> BoatsCollection { get; set; }
        public ObservableCollection<SessionDto> SessionCollection { get; set; }
        public ObservableCollection<DataGps> DataCollection { get; set; }

        public ICommand DrawAction { get; set; }
        public ICommand GetBoatsCommand { get; set; }
        public ICommand ImportExcelDataCommand { get; set; }
        public ICommand SaveToExcelCommand { get; set; }
        public ICommand AcceptDataCommand { get; set; }

        public double WindSpeedMin { get; set; }
        public double WindSpeedMax { get; set; }
        public double WindDirectionMin { get; set; }
        public double WindDirectionMax { get; set; }

        public double OptimalDirection { get; set; }

        List<DataGps> DataGpsList { get; set; }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public DateTime StartDate {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public int SelectedIndexBoat {
            get { return _selectedIndexBoat; }
            set
            {
                _selectedIndexBoat = value;
                GetStartEndDates();
                GetSessions();
            }
        }
    
        public int SelectedIndexSession
        {
            get { return _selectedIndexSession; }
            set
            {
                if (value < 0)
                    _selectedIndexSession = 0;
                else
                    _selectedIndexSession = value;
                GetData();
            }
        }

        public MainWindowViewModel()
        {
            ImportExcelDataCommand = new ActionCommand(ImportExcel);
            SaveToExcelCommand = new ActionCommand(SaveExcel);
            DrawAction = new RelayCommand(DrawChart, IsDataComplete);
            GetBoatsCommand = new ActionCommand(GetBoats);
            AcceptDataCommand = new RelayCommand(AcceptData, IsDataComplete);
            GetBoats();
            SelectedIndexBoat = 0;
            GetStartEndDates();
            GetSessions();
            SelectedIndexSession = 0;
            GetData();
        }

        private void SaveExcel()
        {
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;xlsx";
            if (saveFileDialog.ShowDialog() == true)
                filePath = saveFileDialog.FileName;
        }

        private void ImportExcel()
        {
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;xlsx|All files (*.*)| *.*";
            if (openFileDialog.ShowDialog() == true)
                filePath = openFileDialog.FileName;
        }
                
        private void GetBoats()
        {
            var boatService = new BoatService();
            BoatsCollection = new ObservableCollection<BoatDto>(boatService.GetBoats());
        }

        private void GetStartEndDates()
        {
            var sessionService = new SessionService();
            var selectedBoat = BoatsCollection[SelectedIndexBoat];
            Dictionary<DateTime, DateTime> startEndDates = new Dictionary<DateTime, DateTime>(sessionService.GetStartEndDates(selectedBoat.IdBoat));
            StartDate = startEndDates.Keys.First();
            EndDate = startEndDates.Values.First();
        }

        private void GetSessions()
        {
            var sessionService = new SessionService();
            var selectedBoat = BoatsCollection[SelectedIndexBoat];
            SessionCollection = new ObservableCollection<SessionDto>(sessionService.GetSessions(StartDate, EndDate,selectedBoat.IdBoat));
        }

        private void GetData()
        {
            var dataService = new GpsDataService();
            var selectedSession = SessionCollection[SelectedIndexSession];
            DataCollection = new ObservableCollection<DataGps>(dataService.GetSessions(selectedSession.IdSession));
            if (DataCollection.Count != 0)
                isDataComplete = true;
            else
                isDataComplete = false;


        }
        private bool IsDataComplete(object obj)
        {
            return isDataComplete;
        }

        private void AcceptData(object obj)
        {
            _dataOrigin = "selectedData";

            var gpsDataService = new GpsDataService();
            var selectedSession = SessionCollection[SelectedIndexSession];
            Dictionary<float, float> windSpeedMinMax = new Dictionary<float, float>(gpsDataService.GetWindSpeedMinMax(selectedSession.IdSession));
            WindSpeedMin = windSpeedMinMax.Keys.First();
            WindSpeedMax = windSpeedMinMax.Values.First();
            Dictionary<float, float> windDirectionMinMax = new Dictionary<float, float>(gpsDataService.GetWindDirectionMinMax(selectedSession.IdSession));
            WindDirectionMin = windDirectionMinMax.Keys.First();
            WindDirectionMax = windDirectionMinMax.Values.First();
        }

        private void LoadSelectedData()
        {
            foreach (var item in DataCollection)
            {
                var dataGps = new DataGps();
                dataGps.BoatDirection = item.BoatDirection;
                dataGps.BoatSpeed = item.BoatSpeed;
                DataGpsList.Add(dataGps);
            }
        }

        private void DrawChart(object obj)
        {
            DataGpsList = new List<DataGps>();
            ReadExcelService readExcel;

            switch (_dataOrigin)
            {
                case "excel":
                    readExcel = new ReadExcelService();
                    DataGpsList = readExcel.LoadData(@"K:\Jasiek\Desctop\III_Rok\Projekt_zespolowy\SalingPerformance\SailingPerformance\DaneDoOptymalizacjiŁodzi.xlsx");
                    CalculatePoints();
                    break;
                case "selectedData":
                    LoadSelectedData();
                    CalculatePoints();
                    break;
                default:
                    break;
            }        
        }


        private void CalculatePoints()
        {
            var listToInterpolate = new List<PointD>();

            foreach (var x in DataGpsList)
            {

                double pointX = Math.Cos((90 - x.BoatDirection) / (180 / Math.PI)) * x.BoatSpeed;
                double pointY = Math.Sin((90 - x.BoatDirection) / (180 / Math.PI)) * x.BoatSpeed;
                listToInterpolate.Add(new PointD(pointX, pointY));
            }

            //SplineInterpolator interpolator = new SplineInterpolator(listToInterpolate);
            //var interpolatedList = interpolator.InterpolateCoordinates(listToInterpolate); na razie nie działa!

            ChartViewModel = new ChartViewModel(listToInterpolate);
        }

        //private void CalculatePoints()
        //{
        //    var listToInterpolate = new List<PointD>();
        //    double apparentWind = 0, newApparentWind = 0;

        //    foreach (var x in DataGpsList)
        //    {
        //        var direction = x.WindDirection - x.BoatDirection;
        //        if (direction < 0)
        //            direction = direction * (-1);

        //        //obliczanie wiatru pozornego dla obecnych danych
        //        apparentWind = Math.Sqrt(Math.Pow(x.WindSpeed, 2) + Math.Pow(x.BoatSpeed, 2) + 2 * x.WindSpeed * x.BoatSpeed * Math.Cos(direction));

        //        var newDirection = WindDirection - x.BoatDirection;
        //        if (newDirection < 0)
        //            newDirection = direction * (-1);

        //        //obliczanie wiatru pozornego dla nowych danych
        //        newApparentWind = Math.Sqrt(Math.Pow(WindSpeed, 2) + Math.Pow(x.BoatSpeed, 2) + 2 * WindSpeed * x.BoatSpeed * Math.Cos(newDirection));

        //        double pointX = Math.Cos((90 - x.BoatDirection) / (180 / Math.PI)) * (x.BoatSpeed - (apparentWind - newApparentWind)); //odejmuję różnicę siły wiatru pozornego poprzedniego od nowego
        //        double pointY = Math.Sin((90 - x.BoatDirection) / (180 / Math.PI)) * (x.BoatSpeed - (apparentWind - newApparentWind)); //a potem tą różnicę odejmuję od prędkości łodzi
        //        listToInterpolate.Add(new PointD(pointX, pointY));
        //    }

        //    //SplineInterpolator interpolator = new SplineInterpolator(listToInterpolate);
        //    //var interpolatedList = interpolator.InterpolateCoordinates(listToInterpolate); na razie nie działa!

        //    ChartViewModel = new ChartViewModel(listToInterpolate);
        //}
    }
}
