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
        public OSMService(TransportDbContext db)
        {
            this.db = db;
        }

        public string GetAllRoutes()
        {
            var routes = db.BusLines.Select(x => new
            {
                Name = x.Name,
                ColorHex = String.IsNullOrEmpty(x.ColorHex) ? "#00ffff" : x.ColorHex,
                coordinates = x.Route.OrderBy(y => y.RowPosition).Select(y => new double[] { y.Point.X, y.Point.Y }),
            }) ;
            
            return JsonConvert.SerializeObject(routes);
        }
    }
}
