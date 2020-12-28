using System;
using System.Collections.Generic;
using System.Text;

namespace BusGps.Services.Data
{
    public interface ILocationService
    {
        public void Update(string UserId, double x, double y);
        public void UpdateName(string UserId, string busname);
        public void Remove(string UserId);
        public bool NameIncluded(string UserId);
        public object GetAllDrivers();
        public bool IsCalled { get; set; }
        public bool DriversAvialable { get; }
    }
}
