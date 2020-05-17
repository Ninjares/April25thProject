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
        public object GetRoute(string UserId)
        {
            string defaultColor = "#808080";
            var route = db.Users.Where(x => x.Id == UserId).Select(x => new
            {
                coordinates = x.Bus.Line.Route.OrderBy(y => y.RowPosition).Select(x => new double[] { x.Point.X, x.Point.Y }),
                ColorHex = String.IsNullOrEmpty(x.Bus.Line.ColorHex) ? defaultColor : x.Bus.Line.ColorHex,
            }).FirstOrDefault();
            return route;
        }

        public object GetStops(string UserId)
        {
            var stops = db.Users.Where(x => x.Id == UserId).Select(x => x.Bus.Line.Stops.Select(x => new
            {
                x.BusStop.Address,
                Point = new double[] { x.BusStop.Point.X, x.BusStop.Point.Y }
            }));
            return stops;
        }
    }
}
