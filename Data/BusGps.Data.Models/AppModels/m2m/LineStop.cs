using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusGps.Data.Models.AppModels
{
    public class LineStop
    {
        [Key]
        public string LineId { get; set; }
        [Key]
        public string StopId { get; set; }
        public virtual BusLine Line { get; set; }
        public virtual Stop BusStop { get; set; }
    }
}
