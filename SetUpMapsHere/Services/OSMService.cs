using ASPDbContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public class OSMService : IOSMService
    {
        private readonly TransportDbContext db;
        public OSMService()
        {
            this.db = new TransportDbContext();
        }

        public string GetAllRoutes()
        {
            var routes = db.BusLines.Select(x => new
            {
                type = "PolyLine",
                coordinates = JsonConvert.SerializeObject(x.Route.Select(y => new double[] { y.Point.X, y.Point.Y }), Formatting.Indented)
            });
            return routes.FirstOrDefault().coordinates;
        }
    }
}
