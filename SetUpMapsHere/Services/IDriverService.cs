using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    interface IDriverService
    {
        public string GetStops(string ling);
        public string GetRoute(string line);
    }
}
