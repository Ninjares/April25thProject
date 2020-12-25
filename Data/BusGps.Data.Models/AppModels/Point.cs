using BusGps.Data.Common.Models;
using System.Collections.Generic;

namespace BusGps.Data.Models.AppModels
{
    public class Point : BaseModel<string>
    {
        public Point()
        {
            this.LinePoints = new HashSet<LinePoint>();
        }
        public double X { get; set; }
        public double Y { get; set; }
        public virtual ICollection<LinePoint> LinePoints { get; set; }
    }
}
