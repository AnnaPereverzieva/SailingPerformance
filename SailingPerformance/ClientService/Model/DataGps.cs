using System;

namespace ClientService.Model
{
    public class DataGps
    {
        public DateTime Date { get; set; }
        public double Speed { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double DirectionWind { get; set; }
        public double PointX { get; set; }
        public double PointY { get; set; }
    }
}
