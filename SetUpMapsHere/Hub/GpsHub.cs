namespace SetUpMapsHere.Hub
{
    using ASPDbContext.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using System.Linq;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Threading;
    using System;

    //[Authorize]
    public class GpsHub:Hub
    {
        private ConcurrentDictionary<string, double[]> DriverLocations;
        public UserManager<ApplicationUser> UserManager { get; set; }
        private bool IsTransmitting { get; set; }
        public GpsHub(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            DriverLocations = new ConcurrentDictionary<string, double[]>();
            IsTransmitting = false;
        }

        public void UpdateLocation(double x, double y)
        {
            DriverLocations.AddOrUpdate(UserManager.GetUserId(this.Context.User), new double[] { x, y }, (k,v) => new double[] { x, y });
            this.Clients.All.SendAsync("AllGood", "user", x, y);
        }

        public async Task RemoveBus()
        {
            double[] placeholder;
            string usr = UserManager.GetUserId(this.Context.User);
            DriverLocations.TryRemove(usr, out placeholder);
            await this.Clients.All.SendAsync("RemoveDriver", usr);
        }

        public void Engage()
        {
            if (!IsTransmitting)
            {
                this.Clients.All.SendAsync("Confirm");
                IsTransmitting = true;
                GetAllDrivers();
            }
        }

        public async Task GetAllDrivers()
        {
            while (true)
            {
                await Task.Delay(1000);
                if (DriverLocations.Count != 0)
                {
                    IsTransmitting = true;
                    var buses = DriverLocations.Select(x => new
                    {
                        Id = x.Key,
                        BusLine = UserManager.Users.Where(x => x.Id == UserManager.GetUserId(this.Context.User)).Select(x => x.Bus.Line.Name + " - " + x.Bus.BusLoginHash).FirstOrDefault(),
                        Location = x.Value
                    }).ToArray();
                    //string SignalString = JsonConvert.SerializeObject(buses);
                    await this.Clients.All.SendAsync("DisplayDrivers", buses);
                }
            }
        }
    }
}
