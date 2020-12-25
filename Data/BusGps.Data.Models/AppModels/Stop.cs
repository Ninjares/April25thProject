using BusGps.Data.Common.Models;
using System.Collections.Generic;

namespace BusGps.Data.Models.AppModels
{
    public class Stop : BaseDeletableModel<string>
    {
        public Stop()
        {
            this.LineStops = new HashSet<LineStop>();
        }
        public string Address { get; set; }
        public string PointId { get; set; }
        public virtual Point Point { get; set; }
        public virtual ICollection<LineStop> LineStops { get; set; }
    }
}
