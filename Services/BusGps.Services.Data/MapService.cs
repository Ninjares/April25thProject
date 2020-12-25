using BusGps.Data.Common.Repositories;
using BusGps.Data.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusGps.Services.Data
{
    public class MapService : IMapService
    {
        private readonly string defaultColor = "#808080";
        private readonly IDeletableEntityRepository<BusLine> routes;
        private readonly IDeletableEntityRepository<Stop> stops;

        public MapService(IDeletableEntityRepository<BusLine> routes, IDeletableEntityRepository<Stop> stops)
        {
            this.routes = routes;
            this.stops = stops;
        }

        public object GetAllRoutes(params string[] lines)
        {
            var routes = this.routes.All().Select(x => new
            {
                name = x.Name,
                colorHex = string.IsNullOrEmpty(x.ColorHex) ? this.defaultColor : x.ColorHex,
                coordinates = x.Route.OrderBy(y => y.RowPosition).Select(y => new double[] { y.Point.X, y.Point.Y }),
            }).ToArray();
            return routes;
        }

        public object GetAllStops(params string[] lines)
        {
            var stops = this.stops.All().Select(x => new
            {
                x.Address,
                stopList = string.Join(", ", x.LineStops.Select(y => y.Line.Name)),
                coordinates = new double[] { x.Point.X, x.Point.Y },
            });
            return stops;
        }
    }
}
