using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using PropertyChanged;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class ChartViewModel
    {
        public PlotModel PlotModel { get; set; }
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
            PlotModel = new PlotModel();
            SetUpModel();
        }
        private void SetUpModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;

            //var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Date", "HH:mm") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            //PlotModel.Axes.Add(dateAxis);
            //var valueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            //PlotModel.Axes.Add(valueAxis);
              PlotModel.Axes.Add(new LinearAxis(AxisPosition.Bottom, -20, 80));
             PlotModel.Axes.Add(new LinearAxis(AxisPosition.Left, -10, 10));

        }

    }
}
