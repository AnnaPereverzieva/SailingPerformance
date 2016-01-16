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

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private DateTime _endDate;
        private DateTime _startDate;
        private int _selectedIndexBoat;
        private int _selectedIndexSession;

        public ChartViewModel ChartViewModel { get; set; }
        public ICommand DrawAction { get; set; }

        public ICommand ImportExcelDataCommand { get; set; }
        public ICommand SaveToExcelCommand { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double OptimalDirection { get; set; }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                GetSessions();
            }
        }

        public DateTime StartDate {
            get { return _startDate; }
            set
            {
                _startDate = value;
                GetSessions();
            }
        }

        public int SelectedIndexBoat {
            get { return _selectedIndexBoat; }
            set
            {
                _selectedIndexBoat = value;
                GetSessions();
            }
        }
    
        public int SelectedIndexSession
        {
            get { return _selectedIndexSession; }
            set
            {
                _selectedIndexSession = value;
                GetData();
            }
        }

        public MainWindowViewModel()
        {
            DrawAction=new ActionCommand(DrawChart);
            ImportExcelDataCommand = new ActionCommand(ImportExcel);
            SaveToExcelCommand = new ActionCommand(SaveExcel);
            DrawAction = new ActionCommand(DrawChart);
            GetBoatsCommand = new ActionCommand(GetBoats);
            GetBoats();
            SelectedIndexBoat = 0;
            EndDate = DateTime.Now;
            StartDate = DateTime.Now.AddMonths(-1);
            WindDirection = 10;
            WindSpeed = 2;
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




        public ICommand GetBoatsCommand { get; set; }
        public ObservableCollection<BoatDto> BoatsCollection { get; set; }
        public ObservableCollection<SessionDto> SessionCollection { get; set; }
        public ObservableCollection<DataGps> DataCollection { get; set; }

    

        private void GetBoats()
        {
            var boatService = new BoatService();

            BoatsCollection = new ObservableCollection<BoatDto>(boatService.GetBoats());
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
        }

        private void DrawChart()
        {

            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData(@"C:\Users\malgo\Downloads\DaneDoOptymalizacjiŁodzi.xlsx");
            var listToInterpolate = new List<PointD>();
            double apparentWind = 0, newApparentWind = 0;

            
            foreach (var x in list)
            {
                var direction = x.WindDirection - x.BoatDirection;
                if (direction < 0)
                    direction = direction*(-1);

                //obliczanie wiatru pozornego dla obecnych danych
                apparentWind = Math.Sqrt(Math.Pow(x.WindSpeed,2) + Math.Pow(x.BoatSpeed,2) + 2*x.WindSpeed*x.BoatSpeed*Math.Cos(direction));

                var newDirection = WindDirection - x.BoatDirection;
                if (newDirection < 0)
                    newDirection = direction * (-1);
                
                //obliczanie wiatru pozornego dla nowych danych
                newApparentWind = Math.Sqrt(Math.Pow(WindSpeed, 2) + Math.Pow(x.BoatSpeed, 2) + 2 * WindSpeed * x.BoatSpeed * Math.Cos(newDirection));
                
                double pointX = Math.Cos((90 - x.BoatDirection) / (180 / Math.PI)) * (x.BoatSpeed - (apparentWind - newApparentWind)); //odejmuję różnicę siły wiatru pozornego poprzedniego od nowego
                double pointY = Math.Sin((90 - x.BoatDirection) / (180 / Math.PI)) * (x.BoatSpeed - (apparentWind - newApparentWind)); //a potem tą różnicę odejmuję od prędkości łodzi
                listToInterpolate.Add(new PointD(pointX, pointY));
            }

            //SplineInterpolator interpolator = new SplineInterpolator(listToInterpolate);
            //var interpolatedList = interpolator.InterpolateCoordinates(listToInterpolate); na razie nie działa!

            ChartViewModel = new ChartViewModel(listToInterpolate);
        }
    }
}
