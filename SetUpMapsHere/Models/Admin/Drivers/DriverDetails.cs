using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Models.Admin
{
    public class DriverDetails
    {
        public string DriverId { get; set; }
        public int? BusId { get; set; }
        public string DriverName { get; set; }
        public string CurrentLine { get; set; }
        public string CurrentBus { get; set; }
        public IEnumerable<Option> AllBuses { get; set; }
    }

    public class Option
    {
        public int BusId { get; set; }
        public string BusName { get; set; }
        public string LineName { get; set; }
    }
}
