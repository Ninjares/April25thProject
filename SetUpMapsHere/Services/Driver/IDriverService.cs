using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IDriverService
    {
        public object GetStops(string userId);
        public object GetRoute(string userId);
    }
}
