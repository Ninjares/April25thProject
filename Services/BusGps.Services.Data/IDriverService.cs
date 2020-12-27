using System;
using System.Collections.Generic;
using System.Text;

namespace BusGps.Services.Data
{
    public interface IDriverService
    {
        public object GetStops(string userId);
        public object GetRoute(string userId);
    }
}
