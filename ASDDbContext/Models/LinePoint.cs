using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class LinePoint
    {
        public int LineId { get; set; }
        public int PointId { get; set; }
        public BusLine Line { get; set; }
        public Point Point { get; set; }
        public int RowPosition { get; set; }
    }
}
