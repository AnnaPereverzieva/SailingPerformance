using System;
using System.Collections.Generic;
using ClientService.Model;
using Gui.Common;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Extensions;
using OxyPlot.Series;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class ChartViewModel
    {
        public PlotModel PlotModel { get; set; }
        public LineSeries LineSeries { get; set; }
        private readonly List<OxyColor> Colors = new List<OxyColor>
                                            {
                                                OxyColors.Green,
                                                OxyColors.IndianRed,
                                                OxyColors.Coral,
                                                OxyColors.Chartreuse,
                                                OxyColors.Azure
                                            };

        private readonly List<MarkerType> MarkerTypes = new List<MarkerType>
                                                   {
                                                       MarkerType.Plus,
                                                       MarkerType.Star,
                                                       MarkerType.Diamond,
                                                       MarkerType.Triangle,
                                                       MarkerType.Cross
                                                   };

        public ChartViewModel()
        {
            SetUpModel();
        }

        /// <summary>
        /// Tworzy wykres
        /// </summary>
        /// <param name="list"></param>
        public void DrawChart(List<PointD> list)
        {
            foreach (var point in list)
            {
                LineSeries.Points.Add(new DataPoint(point.X, point.Y));
            }
            PlotModel.Series.Add(LineSeries);
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Dodaje nową serię dla danych i opisuje wykres
        /// </summary>
        /// <param name="list"></param>
        /// <param name="boatsCollection"></param>
        /// <param name="selectedIndexBoat"></param>
        /// <param name="sessionCollection"></param>
        /// <param name="selectedSession"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="windSpeed"></param>
        /// <param name="windDirection"></param>
        /// <param name="allRecords"></param>
        /// <param name="isDataFromExcel"></param>
        public void AddNewSeries(List<PointD> list, ObservableCollection<BoatDto> boatsCollection, int selectedIndexBoat,
            ObservableCollection<SessionDto> sessionCollection, int selectedSession, double minX, double maxX, double minY, double maxY,
            double windSpeed, double windDirection, bool allRecords, bool isDataFromExcel)
        {
            
            LineSeries = new LineSeries();

            if (isDataFromExcel)
                LineSeries.Title = "Dane z excela" + ", kurs opt. z wiatrem: " + Math.Round(maxY, 2).ToString() + ", kurs opt. pod wiatr; " + Math.Round(minY, 2).ToString();
            else
            {
                string sessionDate = sessionCollection[selectedSession].StartDate.Year.ToString() + "/" + sessionCollection[selectedSession].StartDate.Month.ToString() + "/" + sessionCollection[selectedSession].StartDate.Day.ToString();
                LineSeries.Title = boatsCollection[selectedIndexBoat].Name + ", sesja: " + sessionDate +
                                ", kurs opt. z wiatrem: " + Math.Round(maxY, 2).ToString() + ", kurs opt. pod wiatr; " + Math.Round(minY, 2).ToString();
            }
                

            AddAxes(minX - 1, maxX + 1, minY - 1, maxY + 1);

            if (allRecords)
                PlotModel.Title = "Siła wiatru: wszystkie wartości, kierunek wiatru: wszystkie wartości";
            else
                PlotModel.Title = "Siła wiatru: " + windSpeed.ToString() + ", kierunek wiatru: " + windDirection.ToString();

            DrawChart(list);

        }
        /// <summary>
        /// Dodaje układ współrzędych
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        public void AddAxes(double minX, double maxX, double minY, double maxY)
        {
            PlotModel.Axes.Clear();
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,

                Title = "X",
                Minimum = minX,
                Maximum = maxX,

            });

            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Y",
                Minimum = minY,
                Maximum = maxY
            });
        }

        /// <summary>
        /// Przygotowanie ViewModelu
        /// </summary>
        public void SetUpModel()
        {
            PlotModel = new PlotModel();
            PlotModel.LegendTitle = "Legenda";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Inside;
            PlotModel.LegendPosition = LegendPosition.RightTop;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            AddAxes(0, 4, 0, 4);
        }
    }
}
