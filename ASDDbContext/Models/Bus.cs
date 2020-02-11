using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASPDbContext.Models
{
    public class Bus
    {
        public int BusId { get; set; }
        [Required]
        public string BusLoginHash { get; set; }
        public bool IsActive { get; set; }
        public int LineId { get; set; }
        public BusLine Line { get; set; }
    }
}
