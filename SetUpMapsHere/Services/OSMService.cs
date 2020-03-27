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
                coordinates = x.Route.OrderBy(y => y.RowPosition).Select(y => new double[] { y.Point.X, y.Point.Y })
            });
            return JsonConvert.SerializeObject(routes);
        }
    }
}
