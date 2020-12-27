using BusGps.Data.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusGps.Services.Data
{
    public interface IAdminService
    {
        public IEnumerable<object> GetAllBuses();
        public IEnumerable<object> GetAllBusOptions();
        public bool AssignBusToDriver(string userId, int busId);
        public Bus Getbus(int id);
        public bool DismissDriver(string driverId);
        public IEnumerable<object> GetLineOptions();
        public bool CreateBus(string name, int LineId);
        public object GetBusDetails(int id);
        public void EditBus(int id, string name, int lineId);
        public void DeleteBus(int id);
    }
}
