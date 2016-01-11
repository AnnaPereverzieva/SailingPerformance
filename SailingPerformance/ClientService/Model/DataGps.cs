using System;

namespace ClientService.Model
{
    public class DataGps
    {
        public Guid IdSession { get; set; }
        public DateTime SecondsFromStart { get; set; }
        public double BoatSpeed { get; set; }
        public double BoatDirection { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public string GeoHeight { get; set; }
        public string GeoWidth { get; set; }
    }
}
