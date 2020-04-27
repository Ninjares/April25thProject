using ASPDbContext.Models;
using SetUpMapsHere.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IAdminService
    {
        public IEnumerable<BusInfo> GetAllBuses();
        public IEnumerable<Option> GetAllBusOptions();
        public bool AssignBusToDriver(string userId, int busId);
        public Bus Getbus(int id);
        public bool DismissDriver(string driverId);
        public IEnumerable<LineOption> GetLineOptions();
        public bool CreateBus(string name, int LineId);
        public EditBus GetBusDetails(int id);
        public void EditBus(int id, string name, int lineId);
        public void DeleteBus(int id);
    }
}
