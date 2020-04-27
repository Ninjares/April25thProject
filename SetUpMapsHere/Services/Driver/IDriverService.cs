using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IDriverService
    {
        public string GetStops(string userId);
        public string GetRoute(string userId);
    }
}
