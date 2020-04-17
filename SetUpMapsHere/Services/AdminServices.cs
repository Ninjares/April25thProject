using ASPDbContext;
using ASPDbContext.Models;
using SetUpMapsHere.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public class AdminServices : IAdminService
    {
        private TransportDbContext db { get; set; }
        public AdminServices(TransportDbContext db)
        {
            this.db = db;
        }
        public IEnumerable<BusInfo> GetAllBuses()
        {
            return db.Buses.Select(x => new BusInfo
            {
                BusId = x.BusId.ToString(),
                BusName = x.BusLoginHash,
                Line = x.Line.Name,
                IsActive = x.IsActive.ToString()
            }).ToArray();
        }

        public IEnumerable<Option> GetAllBusOptions()
        {
            var buses = db.Buses.Where(x => x.Driver == null).Select(x => new Option
            {
                BusId = x.BusId,
                BusName = x.BusLoginHash,
                LineName = x.Line.Name
            }).ToList();
            return buses;
        }

        public bool AssignBusToDriver(string userId, int busId)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == userId);
            user.BusId = busId;
            db.SaveChanges();
            if (db.Users.FirstOrDefault(x => x.Id == userId).BusId == busId) return true;
            else return false;
        }

        public Bus Getbus(int id)
        {
            var bus = db.Buses.FirstOrDefault(x => x.BusId == id);
            bus.Line = db.BusLines.FirstOrDefault(x => x.Buses.Select(x => x.BusId).Contains(id));
            return bus;
        }

        public bool DismissDriver(string driverId)
        {
            var driver = db.Users.FirstOrDefault(x => x.Id == driverId);
            driver.BusId = null;
            db.SaveChanges();
            if (db.Users.FirstOrDefault(x => x.Id == driverId).BusId == null) return true;
            else return false;
        }

        public IEnumerable<LineOption> GetLineOptions()
        {
            var options = db.BusLines.Select(x => new LineOption
            {
                LineId = x.Id,
                LineName = x.Name
            }).ToList();
            return options;
        }

        public bool CreateBus(string name, int LineId)
        {
            var bus = new Bus()
            {
                BusLoginHash = name,
                LineId = LineId
            };
            db.Buses.Add(bus);
            db.SaveChanges();
            return db.Buses.Contains(bus);
        }

        public EditBus GetBusDetails(int id)
        {
            return new EditBus()
            { 
                Id = id,
                Name = db.Buses.FirstOrDefault(x => x.BusId == id).BusLoginHash,
                LineOptions = GetLineOptions()
            };
        }

        public void EditBus(int id, string name, int lineId)
        {
            var bus = db.Buses.Find(id);
            bus.BusLoginHash = name;
            bus.LineId = lineId;
            db.SaveChanges();
        }

        public void DeleteBus(int id)
        {
            db.Buses.Remove(db.Buses.Find(id));
            db.SaveChanges();
        }
    }
}
