using BusGps.Data.Common.Repositories;
using BusGps.Data.Models.AppModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusGps.Services.Data
{
    public class AdminService : IAdminService
    {
        private readonly IDeletableEntityRepository<Bus> Buses;
        private readonly IDeletableEntityRepository<BusLine> Lines;

        public AdminService(
            IDeletableEntityRepository<Bus> buses,
            IDeletableEntityRepository<BusLine> lines)
        {
            this.Buses = buses;
            this.Lines = lines;
        }

        public bool AssignBusToDriver(string userId, int busId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateBus(string name, string LineId)
        {
            var bus = new Bus()
            {
                Id = Guid.NewGuid().ToString(),
                BusLoginHash = name,
                LineId = LineId,
            };
            await this.Buses.AddAsync(bus);
            await this.Buses.SaveChangesAsync();
            return this.Buses.All().Contains(bus);
        }

        public void DeleteBus(int id)
        {
            throw new NotImplementedException();
        }

        public bool DismissDriver(string driverId)
        {
            throw new NotImplementedException();
        }

        public void EditBus(int id, string name, int lineId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return this.Buses.All().Include(x => x.Line);
        }

        public IEnumerable<object> GetAllBusOptions()
        {
            throw new NotImplementedException();
        }

        public Bus Getbus(string id)
        {
            return this.Buses.All().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BusLine> GetLineOptions()
        {
            return this.Lines.All();
        }
    }
}
