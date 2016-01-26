using System;
using System.Collections.Generic;

namespace ClientService.Responses
{
    public class GpsSingleByDateResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Date { get; set; }     
        public List<string> Latitude { get; set; }      
        public List<string> Longitude { get; set; }      
        public List<DateTime> Time { get; set; }      
        public List<string> StrengthWind { get; set; }      
        public List<string> DirectionWind { get; set; }
    }
}
