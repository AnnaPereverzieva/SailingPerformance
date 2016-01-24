using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Gui.Common;
using Microsoft.Expression.Interactivity.Core;
using OxyPlot;
using OxyPlot.Wpf;
using PropertyChanged;
using Spire.Xls;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private DateTime _endDate;
        private DateTime _startDate;
        private int _selectedIndexBoat;
        private int _selectedIndexSession;
        private double _windSpeed;
        private double _windDirection;
        private bool _isWindSelected;
        private bool _allRecords;
        private bool _isDataFromExcel;

        private double _minX, _minY, _maxX, _maxY;

        public bool IsAccepted { get; set; }
        public bool IsDataChanged { get; set; }
        public bool WindValuesChanged { get; set; }

        public bool AllRecords
        {
            get { return _allRecords; }
            set
            {
                _allRecords = value;
                WindValuesChanged = true;
                if (!_isDataFromExcel)
                    GetData();

                AvailableRecords = DataCollection.Count();
                IsDataChanged = true;
            }
        }


        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                GetSessions();
            }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                GetSessions();
            }
        }
        public int SelectedIndexBoat
        {
            get { return _selectedIndexBoat; }
            set
            {
                if (_selectedIndexBoat != value)
                {
                    _selectedIndexBoat = value;
                    GetStartEndDates();
                    GetSessions();
                    IsDataChanged = true;
                }
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
                {
                    if (_selectedIndexSession != value)
                    {
                        _selectedIndexSession = value;
                        IsDataChanged = true;
                    }
                }
                IsAccepted = false;
                GetData();
            }
        }

        public int AvailableRecords { get; set; }
        public double WindSpeed
        {
            get { return Math.Round(_windSpeed, 2); }
            set
            {
                _windSpeed = value;
                WindValuesChanged = true;
                AvailableWindSpeed = CheckAvailableWindRecords(AvalableWindSpeedList, _windSpeed);
                CheckTotalNumberOfRecords();
                IsDataChanged = true;
            }
        }
        public double WindSpeedMin { get; set; }
        public double WindSpeedMax { get; set; }
        public double AvailableWindSpeed { get; set; }
        public double WindDirection
        {
            get { return Math.Round(_windDirection, 2); }
            set
            {
                _windDirection = value;
                WindValuesChanged = true;
                AvailableWindDirection = CheckAvailableWindRecords(AvalableWindDirectionList, _windDirection);
                CheckTotalNumberOfRecords();
                IsDataChanged = true;
            }
        }
        public double WindDirectionMin { get; set; }
        public double WindDirectionMax { get; set; }
        public double AvailableWindDirection { get; set; }
        public List<double> AvalableWindSpeedList { get; set; }
        public List<double> AvalableWindDirectionList { get; set; }

        public double OptimalDirection { get; set; }
        public ChartViewModel ChartViewModel { get; set; }
        public ObservableCollection<BoatDto> BoatsCollection { get; set; }
        public ObservableCollection<SessionDto> SessionCollection { get; set; }
        public ObservableCollection<DataGps> DataCollection { get; set; }
        public ObservableCollection<DataGps> ExcelDataCollection { get; set; }

        public ICommand DrawPlotCommand { get; set; }
        public ICommand ClearPlotCommand { get; set; }
        public ICommand GetBoatsCommand { get; set; }
        public ICommand ImportExcelDataCommand { get; set; }
        public ICommand SaveToExcelCommand { get; set; }
        public ICommand AcceptDataCommand { get; set; }
        public ICommand RefreshDataCommand { get; set; }
        public ICommand SaveToPdfCommand { get; set; }
        

        public MainWindowViewModel()
        {
            ChartViewModel = new ChartViewModel();
            ImportExcelDataCommand = new ActionCommand(ImportExcel);
            SaveToExcelCommand = new ActionCommand(SaveExcel);
            DrawPlotCommand = new RelayCommand(DrawChart, CheckDataComplete);
            ClearPlotCommand = new ActionCommand(ClearPlot);
            GetBoatsCommand = new ActionCommand(GetBoats);
            AcceptDataCommand = new RelayCommand(AcceptData, DataComplete);
            RefreshDataCommand = new ActionCommand(RefreshData);
            SaveToPdfCommand = new ActionCommand(SaveToPdf);
            GetBoats();
            SelectedIndexBoat = 0;
            GetStartEndDates();
            GetSessions();
            GetData();
            IsDataChanged = true;
            InitiateAxisValues();
        }

        private void InitiateAxisValues()
        {
            _minX = 0;
            _minY = 0;
            _maxX = 0;
            _maxY = 0;
        }

        private void SaveToPdf()
        {
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
                filePath = saveFileDialog.FileName;
            //using (var stream = File.Create(filePath)) // pdf file zle zapisuje, nie zna polskich znakow, encodingu niema 
            //{
            //    PdfExporter.Export(ChartViewModel.PlotModel, stream, 600, 400);
            //}
            using (var stream = File.Create(filePath))
            {
                PngExporter.Export(ChartViewModel.PlotModel, stream, 750, 550, OxyColor.FromArgb(250, 250, 250, 250));
            }
        }

        private void SaveExcel()
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            int row = 2;
            var obj = DataCollection.FirstOrDefault();
            if (obj == null) return;
            sheet.Range["A" + 1].Text = nameof(obj.GeoHeight);
            sheet.Range["B" + 1].Text = nameof(obj.GeoWidth);
            sheet.Range["C" + 1].Text = nameof(obj.BoatDirection);
            sheet.Range["D" + 1].Text = nameof(obj.BoatSpeed);
            sheet.Range["E" + 1].Text = nameof(obj.WindDirection);
            sheet.Range["F" + 1].Text = nameof(obj.WindSpeed);
            sheet.Range["G" + 1].Text = nameof(obj.SecondsFromStart);

            foreach (var item in DataCollection)
            {
                sheet.Range["A" + row].Text = item.GeoHeight;
                sheet.Range["B" + row].Text = item.GeoWidth;
                sheet.Range["C" + row].Text = item.BoatDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["D" + row].Text = item.BoatSpeed.ToString(CultureInfo.InvariantCulture);
                sheet.Range["E" + row].Text = item.WindDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["F" + row].Text = item.WindDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["G" + row].Text = item.SecondsFromStart.ToString(CultureInfo.InvariantCulture);
                row++;
            }
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
                filePath = saveFileDialog.FileName;

            workbook.SaveToFile(filePath);
            System.Diagnostics.Process.Start(workbook.FileName);

        }

        private void ImportExcel()
        {
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)| *.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                ReadExcelService readExcel = new ReadExcelService();
                ExcelDataCollection = new ObservableCollection<DataGps>(readExcel.LoadData(filePath));
                DataCollection = new ObservableCollection<DataGps>(ExcelDataCollection);
                _isWindSelected = false;
                GetWindParameters();
                _isDataFromExcel = true;
                ClearPlot();
            }
        }      

        private void ClearPlot()
        {
            ChartViewModel = new ChartViewModel();
        }

        private void RefreshData()
        {
            GetBoats();
            GetStartEndDates();
            GetSessions();
            GetData();
        }

        private void CheckTotalNumberOfRecords()
        {
            AvailableRecords = DataCollection.Where(x => x.WindDirection == AvailableWindDirection && x.WindSpeed == AvailableWindSpeed).Count();
        }

        //sprawdza czy istnieje wystarczjaca ilosc rekordow dla podanych parametrow wiatru (minimalNoRecords ustawia min ilosc rekordow)
        private double CheckAvailableWindRecords(List<double> availableWindList, double windValue)
        {
            double closest = 0;
            int minimalNoRecords = 3;

            if (availableWindList.Where(x => x.Equals(windValue)).Count() > minimalNoRecords)
            {
                return windValue;
            }
            else
            {
                List<double> windValues = new List<double>(availableWindList.Distinct());
                windValues.Sort();
                int noOccurances = 0;
                do
                {
                    closest = windValues.Aggregate((x, y) => Math.Abs(x - windValue) < Math.Abs(y - windValue) ? x : y);
                    noOccurances = availableWindList.Where(x => x.Equals(closest)).Count();
                    windValues.Remove(closest);
                    if (!windValues.Any())
                    {
                        return 0;
                    }
                } while (noOccurances < minimalNoRecords);
            }
            return closest;
        }
        
        private void GetBoats()
        {
            var boatService = new BoatService();
            BoatsCollection = new ObservableCollection<BoatDto>(boatService.GetBoats());
        }

        private void GetStartEndDates()
        {
            try
            {
                var sessionService = new SessionService();
                var selectedBoat = BoatsCollection[SelectedIndexBoat];
                Dictionary<DateTime, DateTime> startEndDates = new Dictionary<DateTime, DateTime>(sessionService.GetStartEndDates(selectedBoat.IdBoat));
                StartDate = startEndDates.Keys.First();
                EndDate = startEndDates.Values.First();
            }
            catch (Exception)
            { }
        }

        private void GetSessions()
        {
            try
            {
                var sessionService = new SessionService();
                var selectedBoat = BoatsCollection[SelectedIndexBoat];
                SessionCollection = new ObservableCollection<SessionDto>(sessionService.GetSessions(StartDate, EndDate, selectedBoat.IdBoat));
            }
            catch (Exception)
            { }
        }

        private void GetData()
        {
            try
            {
                var dataService = new GpsDataService();
                var selectedSession = SessionCollection[SelectedIndexSession];
                DataCollection = new ObservableCollection<DataGps>(dataService.GetSessions(selectedSession.IdSession));
            }
            catch (Exception)
            { }
        }

        private void AcceptData(object obj)
        {
            if (_isDataFromExcel)
            {
                _isWindSelected = false;
                _isDataFromExcel = false;
            }

            GetData();

            if (_isWindSelected)
            {
                CheckTotalNumberOfRecords();
                if (AvailableRecords == 0)
                {
                   MessageBoxResult result = MessageBox.Show("Podana sesja nie zawiera parametrów wiatru podanych dla wcześniejszej sejsi."+ 
                       " Niemożliwe jest dodanie wykresu do obecnie istniejącego. Potwierdzenie oznacza wykasowanie obecengo wykresu.", 
                       "Uwaga", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.OK)
                    {
                        ClearPlot();
                        _isWindSelected = false;
                    }
                }
            }

            GetWindParameters();
            IsAccepted = true;
        }

        private bool DataComplete(object obj)
        {
            if (DataCollection.Count != 0 && IsDataChanged && !IsAccepted)
                return true;
            else
                return false;

        }

        private bool CheckDataComplete(object obj)
        {
            if (AvailableRecords > 0 && IsDataChanged && IsAccepted)
                return true;

            return false;
        }

        private void GetWindParameters()
        {
            WindSpeedMin = DataCollection.Select(x => x.WindSpeed).Min();
            WindSpeedMax = DataCollection.Select(x => x.WindSpeed).Max();
            WindDirectionMin = DataCollection.Select(x => x.WindDirection).Min();
            WindDirectionMax = DataCollection.Select(x => x.WindDirection).Max();

            AvalableWindSpeedList = new List<double>(DataCollection.Select(x => x.WindSpeed));
            AvalableWindDirectionList = new List<double>(DataCollection.Select(x => x.WindDirection));
            AvalableWindSpeedList.Sort();
            AvalableWindDirectionList.Sort();

            if (!_isWindSelected)
            {
                AvailableWindSpeed = CheckAvailableWindRecords(AvalableWindSpeedList, _windSpeed);
                AvailableWindDirection = CheckAvailableWindRecords(AvalableWindDirectionList, _windDirection);
                CheckTotalNumberOfRecords();
                WindDirection = AvailableWindDirection;
                WindSpeed = AvailableWindSpeed;
                _isWindSelected = true;
            }
        }
        
        private void DrawChart(object obj)
        {
            IsDataChanged = false;
            if (WindValuesChanged)
                ClearPlot();

            WindDirection = AvailableWindDirection;
            WindSpeed = AvailableWindSpeed;
            WindValuesChanged = false;

            var listToInterpolate = new List<PointD>();

            double distaceFromAxisStart = 0, maxDistance = 0, optimalDirection = 0;

            if (!AllRecords)
                DataCollection = new ObservableCollection<DataGps>(DataCollection.Where(x => x.WindSpeed == WindSpeed && x.WindDirection == WindDirection));


            foreach (var x in DataCollection)
            {
                double pointX = Math.Cos((90 - x.BoatDirection) / (180 / Math.PI)) * x.BoatSpeed;
                double pointY = Math.Sin((90 - x.BoatDirection) / (180 / Math.PI)) * x.BoatSpeed;

                listToInterpolate.Add(new PointD(pointX, pointY));

                FindMinMaxAxis(listToInterpolate);

                // liczy odległość od początku ukłądu współrzędnych
                // tam gdzie odległość jest największa kurs jest optymalny
                distaceFromAxisStart = Math.Sqrt(Math.Pow(pointX, 2) + Math.Pow(pointY, 2));
                if (distaceFromAxisStart > maxDistance)
                {
                    maxDistance = distaceFromAxisStart;
                    optimalDirection = x.BoatDirection;
                }
            }

            //SplineInterpolator interpolator = new SplineInterpolator(listToInterpolate);
            //var interpolatedList = interpolator.InterpolateCoordinates(listToInterpolate,0.1); //nie działa!
            OptimalDirection = optimalDirection;
            ChartViewModel.AddNewSeries(listToInterpolate, BoatsCollection, SelectedIndexBoat, SessionCollection, SelectedIndexSession,
                _minX, _maxX, _minY, _maxY, AvailableWindSpeed, AvailableWindDirection, AllRecords, _isDataFromExcel);

            if (_isDataFromExcel)
                DataCollection = new ObservableCollection<DataGps>(ExcelDataCollection);
            else
                GetData();
        }

        private void FindMinMaxAxis(List<PointD> listToInterpolate)
        {
            double tempMinX = listToInterpolate.Select(m => m.X).Min();
            double tempMaxX = listToInterpolate.Select(m => m.X).Max();
            double tempMinY = listToInterpolate.Select(m => m.Y).Min();
            double tempMaxY = listToInterpolate.Select(m => m.Y).Max();

            _minX = _minX > tempMinX ? tempMinX : _minX;
            _minY = _minY > tempMinY ? tempMinY : _minY;
            _maxX = _maxX < tempMaxX ? tempMaxX : _maxX;
            _maxY = _maxY < tempMaxY ? tempMaxY : _maxY;
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
