using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusGps.Web.ViewModels.Admin
{
    public class DriverDetails
    {
        public string DriverId { get; set; }
        public string BusId { get; set; }
        public string DriverName { get; set; }
        public string CurrentLine { get; set; }
        public string CurrentBus { get; set; }
        public IEnumerable<Option> AllBuses { get; set; }
    }

    public class Option
    {
        public string BusId { get; set; }
        public string BusName { get; set; }
        public string LineName { get; set; }
    }
}
