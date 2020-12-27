using BusGps.Data.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusGps.Services.Data
{
    public interface IAdminService
    {
        public IEnumerable<Bus> GetAllBuses();
        public IEnumerable<object> GetAllBusOptions();
        public bool AssignBusToDriver(string userId, int busId);
        public Bus Getbus(string id);
        public bool DismissDriver(string driverId);
        public IEnumerable<BusLine> GetLineOptions();
        public Task<bool> CreateBus(string name, string LineId);
        public void EditBus(int id, string name, int lineId);
        public void DeleteBus(int id);
    }
}
