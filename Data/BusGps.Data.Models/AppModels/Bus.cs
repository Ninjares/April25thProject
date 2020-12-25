using BusGps.Data.Common.Models;

namespace BusGps.Data.Models.AppModels
{
    public class Bus : BaseDeletableModel<string>
    {
        public string BusLoginHash { get; set; }
        public bool IsActive { get; set; }
        public string LineId { get; set; }
        public virtual BusLine Line { get; set; }
        public virtual ApplicationUser Driver { get; set; }
    }
}
