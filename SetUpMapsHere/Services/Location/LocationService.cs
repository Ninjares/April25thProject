using ASPDbContext;
using ASPDbContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public class LocationService : ILocationService
    {
        private ConcurrentDictionary<string, double[]> DriverLocations;
        private ConcurrentDictionary<string, string> DriverBusname;
        
        public bool IsCalled { get; set; }
        public bool DriversAvialable { get => DriverLocations.Count != 0; }

        //private readonly IServiceScopeFactory scopeFactory;
        public LocationService()
        {
            DriverLocations = new ConcurrentDictionary<string, double[]>();
            DriverBusname = new ConcurrentDictionary<string, string>();
            IsCalled = false;
            //How do I inject a database in a singleton?
            using(var db = new TransportDbContext())
            {
                var data = db.Users.Where(x => x.BusId.HasValue).Select(x => new
                {
                    x.Id,
                    BusName = x.Bus.Line.Name + " - " + x.Bus.BusLoginHash
                });
                foreach(var pair in data)
                {
                    DriverBusname.AddOrUpdate(pair.Id, pair.BusName, (k, v) => pair.BusName);
                }
            }

        }

        public object GetAllDrivers()
        {
            {
                var buses = DriverLocations.Select(x => new
                {
                    Id = x.Key,
                    BusLine = DriverBusname[x.Key],
                    Location = x.Value
                }).ToArray();
                return buses;
            }
        }

        public void Remove(string UserId)
        {
            bool deleted = false;
            double[] placeholder;
            while (!deleted)
            {
                deleted = DriverLocations.TryRemove(UserId, out placeholder);
            }
        }

        public void Update(string UserId, double x, double y)
        {
            DriverLocations.AddOrUpdate(UserId, new double[] { x, y }, (k, v) => new double[] { x, y });
            Console.WriteLine();
        }
    }

}
