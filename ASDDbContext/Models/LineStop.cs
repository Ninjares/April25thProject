using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class LineStop
    {
        public int LineId { get; set; }
        public int StopId { get; set; }
        public BusLine Line { get; set; }
        public BusStop BusStop { get; set; }
    }
}
