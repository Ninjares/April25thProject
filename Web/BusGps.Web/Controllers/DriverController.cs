using BusGps.Data.Models;
using BusGps.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusGps.Web.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDriverService driverService;

        public DriverController(
            UserManager<ApplicationUser> userManager,
            IDriverService driverService)
        {
            this.userManager = userManager;
            this.driverService = driverService;
        }

        [HttpGet("Driver/DriverView")]
        public IActionResult Drive()
        {
            return this.View("DriverView");
        }

        [HttpGet("Driver/Stops")]
        public Task<JsonResult> GetStops()
        {
            return Task.FromResult(this.Json(this.driverService.GetStops(this.userManager.GetUserId(this.User))));
        }

        [HttpGet("Driver/Route")]
        public Task<JsonResult> GetRoute()
        {
            return Task.FromResult(this.Json(this.driverService.GetRoute(this.userManager.GetUserId(this.User))));
        }
    }
}
