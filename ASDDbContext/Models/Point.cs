using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class Point
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        
        public ICollection<LinePoint> LinePoints { get; set; }
    }
}
