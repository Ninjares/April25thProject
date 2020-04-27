using ASPDbContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SetUpMapsHere.Models.Driver;
using SetUpMapsHere.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Controllers
{
    //[Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private IDriverService DriverService { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        public DriverController(UserManager<ApplicationUser> userManager, IDriverService service)
        {
            DriverService = service;
            UserManager = userManager;
        }
        [HttpGet("Driver/DriverView")]
        public IActionResult Drive()
        {
            string userId = UserManager.GetUserId(this.User);
            var model = new DrivingMode()
            {
                 Route = DriverService.GetRoute(userId),
                 Stops = DriverService.GetStops(userId)
            };
            return View("DriverView", model);
        }
    }
}
