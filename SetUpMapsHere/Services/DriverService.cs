using ASPDbContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public class DriverService : IDriverService
    {
        private TransportDbContext db;
        public DriverService(TransportDbContext db)
        {
            this.db = db;
        }
        public string GetRoute(string line)
        {
            var route = db.BusLines.FirstOrDefault(x => x.Name == line);
            return JsonConvert.SerializeObject(route);
        }

        public string GetStops(string line)
        {
            var stops = db.BusLines.FirstOrDefault(x => x.Name == line).Stops;
            return JsonConvert.SerializeObject(stops);
        }
    }
}
