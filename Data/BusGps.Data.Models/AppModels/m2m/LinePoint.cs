using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusGps.Data.Models.AppModels
{
    public class LinePoint
    {
        [Key]
        public string LineId { get; set; }
        [Key]
        public string PointId { get; set; }
        public virtual BusLine Line { get; set; }
        public virtual Point Point { get; set; }
        [Key]
        public int RowPosition { get; set; }
    }
}
