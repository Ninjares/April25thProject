using BusGps.Data.Common.Repositories;
using BusGps.Data.Models;
using BusGps.Data.Models.AppModels;
using BusGps.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusGps.Web.Hubs
{
    public class GpsHub : Hub
    {

        public UserManager<ApplicationUser> UserManager { get; set; }
        public ILocationService LocationService { get; set; }
        public IDeletableEntityRepository<Bus> Buses { get; set; }
        public GpsHub(UserManager<ApplicationUser> userManager, ILocationService service, IDeletableEntityRepository<Bus> buses)
        {
            this.UserManager = userManager;
            this.LocationService = service;
            this.Buses = buses;
        }
        [Authorize(Roles = "Driver")]
        public async Task UpdateLocation(double x, double y)
        {
            string UserId = this.UserManager.GetUserId(this.Context.User);
            if (!this.LocationService.NameIncluded(UserId))
            { 
                var bus = this.Buses.All().Include(x => x.Driver).Include(x => x.Line).FirstOrDefault(x => x.Driver.Id == UserId);
                string name = $"{bus.Line.Name} - {bus.BusLoginHash}";
                LocationService.UpdateName(UserId, name);
            };
            LocationService.Update(UserId, x, y);

            //await this.Clients.All.SendAsync("AllGood", "user", x, y);
            //Task.Factory.StartNew(() => GetAllDrivers(), TaskCreationOptions.RunContinuationsAsynchronously);
            //Console.WriteLine();
        }
        [Authorize(Roles = "Driver")]
        public async Task RemoveBus()
        {
            string usr = UserManager.GetUserId(this.Context.User);
            LocationService.Remove(usr);
            await this.Clients.All.SendAsync("RemoveDriver", usr);
        }

        public async Task GetAllDrivers()
        {
            if (!LocationService.IsCalled)
            {

                LocationService.IsCalled = true;
                while (true)
                {
                    Task t = Task.Delay(1000);
                    {
                        await this.Clients.All.SendAsync("InvokeLocation");
                        if (LocationService.DriversAvialable)
                        {
                            var buses = LocationService.GetAllDrivers();
                            await this.Clients.All.SendAsync("DisplayDrivers", buses);
                        }
                    }
                    t.Wait();
                }
            }
        }
    }
}
