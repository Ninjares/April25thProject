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
        public IEnumerable<Bus> GetAllBusOptions();
        public Task<bool> AssignBusToDriver(string userId, string busId);
        public Bus Getbus(string id);
        public Task<bool> DismissDriver(string driverId);
        public IEnumerable<BusLine> GetLineOptions();
        public Task<bool> CreateBus(string name, string LineId);
        public Task EditBus(string id, string name, string lineId);
        public Task DeleteBus(string id);
    }
}
