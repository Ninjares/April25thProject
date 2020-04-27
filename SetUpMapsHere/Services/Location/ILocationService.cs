using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface ILocationService
    {
        public void Update(string UserId, double x, double y);
        public void Remove(string UserId);
        public object GetAllDrivers();
        public bool IsCalled { get; set; }
        public bool DriversAvialable { get; }
    }
}
