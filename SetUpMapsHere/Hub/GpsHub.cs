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

    //[Authorize]
    public class GpsHub:Hub
    {
        private ConcurrentDictionary<string, double[]> DriverLocations;
        public UserManager<ApplicationUser> UserManager { get; set; }
        public GpsHub(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            DriverLocations = new ConcurrentDictionary<string, double[]>();
        }

        public async Task UpdateLocation(double x, double y)
        {
            DriverLocations.AddOrUpdate(UserManager.GetUserId(this.Context.User), new double[] { x, y }, (k,v) => new double[] { x, y });
            await GetAllDrivers();
        }

        public async Task GetAllDrivers()
        {
            var buses = DriverLocations.Select(x => new
            {
                BusLine = UserManager.Users.Where(x => x.Id == UserManager.GetUserId(this.Context.User)).Select(x => x.Bus.Line.Name + " - " + x.Bus.BusLoginHash).FirstOrDefault(),
                Location = x.Value
            });
            string SignalString = JsonConvert.SerializeObject(buses);
            await this.Clients.All.SendAsync("DisplayDrivers", buses);
        }
    }
}
