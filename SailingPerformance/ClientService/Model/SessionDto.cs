using System;

namespace ClientService.Model
{
    public class SessionDto
    {
        public Guid IdSession { get; set; }
        public Guid IdBoat { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
