using BusGps.Data.Common.Repositories;
using BusGps.Data.Models;
using BusGps.Data.Models.AppModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;

        public AdminService(
            IDeletableEntityRepository<Bus> buses,
            IDeletableEntityRepository<BusLine> lines,
            UserManager<ApplicationUser> userManager)
        {
            this.Buses = buses;
            this.Lines = lines;
            this.userManager = userManager;
        }

        public async Task<bool> AssignBusToDriver(string userId, string busId)
        {
            var user = await userManager.FindByIdAsync(userId);
            user.BusId = busId;
            await userManager.UpdateAsync(user);
            return userManager.FindByIdAsync(userId).Result.BusId == busId;
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

        public async Task DeleteBus(string id)
        {
            this.Buses.Delete(this.Getbus(id));
            await this.Buses.SaveChangesAsync();
        }

        public async Task<bool> DismissDriver(string driverId)
        {
            var user = await userManager.FindByIdAsync(driverId);
            user.BusId = null;
            await userManager.UpdateAsync(user);
            return userManager.FindByIdAsync(driverId).Result.BusId == null;
        }

        public async Task EditBus(string id, string name, string lineId)
        {
            var bus = this.Getbus(id);
            bus.BusLoginHash = name;
            bus.LineId = lineId;
            this.Buses.Update(bus);
            await this.Buses.SaveChangesAsync();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return this.Buses.All().Include(x => x.Line);
        }

        public IEnumerable<Bus> GetAllBusOptions()
        {
            return this.Buses.All().Include(x => x.Driver).Include(x => x.Line).Where(x => x.Driver == null);
        }

        public Bus Getbus(string id)
        {
            return this.Buses.All().Include(x => x.Line).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BusLine> GetLineOptions()
        {
            return this.Lines.All();
        }
    }
}
