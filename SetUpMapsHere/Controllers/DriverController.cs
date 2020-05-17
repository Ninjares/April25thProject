using ASPDbContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SetUpMapsHere.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Controllers
{
    [Authorize(Roles = "Driver")]
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
            return View("DriverView");
        }
        [HttpPost("Driver/Stops")]
        public async Task<JsonResult> GetStops()
        {
            return this.Json(DriverService.GetStops(UserManager.GetUserId(this.User)));
        }
        [HttpPost("Driver/Route")]
        public async Task<JsonResult> GetRoute()
        {
            return this.Json(DriverService.GetRoute(UserManager.GetUserId(this.User)));
        }
    }
}
