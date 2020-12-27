using BusGps.Data.Common.Repositories;
using BusGps.Data.Models;
using BusGps.Data.Models.AppModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusGps.Services.Data
{
    public class DriverService:IDriverService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<BusLine> lines;
        private readonly IDeletableEntityRepository<Stop> stops;
        private const string DefaultColor = "#808080";

        public DriverService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<BusLine> lines,
            IDeletableEntityRepository<Stop> stops)
        {
            this.userManager = userManager;
            this.lines = lines;
            this.stops = stops;
        }

        public object GetRoute(string userId)
        {
            var route = this.lines.All()
                .Include(x => x.Route).ThenInclude(x => x.Point)
                .Include(x => x.Buses).ThenInclude(x => x.Driver)
                .Single(x => x.Buses.Any(x => x.Driver.Id == userId));
            return new
            {
                coordinates = route.Route.OrderBy(x => x.RowPosition).Select(x => new double[] { x.Point.X, x.Point.Y }),
                ColorHex = string.IsNullOrEmpty(route.ColorHex) ? DefaultColor : route.ColorHex,
            };
        }

        public object GetStops(string userId)
        {
            var stops = this.stops.All()
                .Include(x => x.Point)
                .Include(x => x.LineStops).ThenInclude(x => x.Line).ThenInclude(x => x.Buses).ThenInclude(x => x.Driver)
                .Where(x => x.LineStops.Any(x => x.Line.Buses.Any(x => x.Driver.Id == userId)))
                .Select(x => new
                {
                    x.Address,
                    Point = new double[] {x.Point.X, x.Point.Y },
                });
            return stops;
        }
    }
}
