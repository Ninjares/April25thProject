using ASPDbContext;
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
    }
}
