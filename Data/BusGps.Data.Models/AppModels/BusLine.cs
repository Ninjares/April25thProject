using BusGps.Data.Common.Models;
using System.Collections.Generic;

namespace BusGps.Data.Models.AppModels
{
    public class BusLine : BaseDeletableModel<string>
    {
        public BusLine()
        {
            this.Stops = new HashSet<LineStop>();
            this.Route = new HashSet<LinePoint>();
            this.Buses = new HashSet<Bus>();
        }
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public virtual ICollection<LineStop> Stops { get; set; }
        public virtual ICollection<LinePoint> Route { get; set; }
        public virtual ICollection<Bus> Buses { get; set; }
    }
}
