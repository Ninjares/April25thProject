using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASPDbContext.Models
{
    public class BusLine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public string ColorHex { get; set; }
        public ICollection<LineStop> Stops { get; set; }
        public ICollection<LinePoint> Route { get; set; }
        public ICollection<Bus> Buses { get; set; }
    }
}
