using BusGps.Data.Models;
using BusGps.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        public GpsHub(UserManager<ApplicationUser> userManager, ILocationService service)
        {
            UserManager = userManager;
            LocationService = service;
        }
        [Authorize(Roles = "Driver")]
        public async Task UpdateLocation(double x, double y)
        {
            string UserId = UserManager.GetUserId(this.Context.User);
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
