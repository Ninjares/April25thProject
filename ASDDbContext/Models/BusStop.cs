using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class BusStop
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int PointId { get; set; }
        public Point Point { get; set; }
        public ICollection<LineStop> LineStops { get; set; }
    }
}
