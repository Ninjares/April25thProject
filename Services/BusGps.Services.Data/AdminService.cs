using BusGps.Data.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusGps.Services.Data
{
    public class AdminService : IAdminService
    {
        public bool AssignBusToDriver(string userId, int busId)
        {
            throw new NotImplementedException();
        }

        public bool CreateBus(string name, int LineId)
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(int id)
        {
            throw new NotImplementedException();
        }

        public bool DismissDriver(string driverId)
        {
            throw new NotImplementedException();
        }

        public void EditBus(int id, string name, int lineId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllBuses()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllBusOptions()
        {
            throw new NotImplementedException();
        }

        public Bus Getbus(int id)
        {
            throw new NotImplementedException();
        }

        public object GetBusDetails(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetLineOptions()
        {
            throw new NotImplementedException();
        }
    }
}
