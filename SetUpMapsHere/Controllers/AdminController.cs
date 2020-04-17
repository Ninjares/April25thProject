using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPDbContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SetUpMapsHere.Models.Admin;
using SetUpMapsHere.Services;

namespace SetUpMapsHere.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IAdminService AdminServices { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private RoleManager<ApplicationRole> RoleManager { get; set; }
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IAdminService services)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            AdminServices = services;
        }
        [HttpGet("/Admin/Drivers/AllDrivers")]
        public IActionResult AllDrivers()
        {
            var model = UserManager.Users.Where(x => x.UserRoles.Any(z => z.Role.Name == "Driver")).Select(x => new DriverInfo
            {
                Username = x.UserName,
                Bus = x.Bus == null ? "Not assigned" : x.Bus.BusLoginHash,
                Line = x.Bus == null ? "Not assigned" : x.Bus.Line.Name,
                Id = x.Id
            }).ToList();
            return View("Drivers/AllDrivers", model);
        }
        [HttpGet("/Admin/Roles/AllUsers")]
        public IActionResult AllUsers()
        {
            var model = UserManager.Users.Select(x => new UserInfo
            {
                Username = x.UserName,
                Email = x.Email,
                Id = x.Id,
                Role = x.UserRoles.FirstOrDefault().Role.Name
            }).ToList();
            return View("Roles/AllUsers", model);
        }

        [HttpGet("/Admin/Buses/AllBuses")]
        public IActionResult AllBuses()
        {
            var model = new List<BusInfo>(AdminServices.GetAllBuses());
            return View("Buses/AllBuses", model);
        }

        [HttpGet("/Admin/Buses/CreateBus")]
        public IActionResult CreateBus()
        {
            var model = AdminServices.GetLineOptions();
            return View("Buses/CreateBus", model);
        }
        [HttpPost("/Admin/Buses/CreateBus")]
        public IActionResult CreateBusPost(string BusName, int LineId)
        {
            bool result = AdminServices.CreateBus(BusName, LineId);
            if (result)
                return Redirect("/Admin/Buses/AllBuses");
            else throw new Exception("Failed Creation");
        }
        [HttpGet("/Admin/Buses/EditBus")]
        public IActionResult EditBus(int BusId)
        {
            var model = AdminServices.GetBusDetails(BusId);
            return View("Buses/EditBus", model);
        }
        [HttpPost("/Admin/Buses/EditBus")]
        public IActionResult EditBusPost(int BusId, string BusName, int LineId)
        {
            AdminServices.EditBus(BusId, BusName, LineId);
            return Redirect("/Admin/Buses/AllBuses");
        }
        [HttpPost("/Admin/Buses/DeleteBus")]
        public IActionResult DeleteBus(int BusId)
        {
            AdminServices.DeleteBus(BusId);
            return Redirect("/Admin/Buses/AllBuses");
        }

        [HttpGet("/Admin/Drivers/EditDriver")]
        public async Task<IActionResult> EditDriver(string DriverId)
        {
            var user = await UserManager.FindByIdAsync(DriverId);
            Bus bus = null;
            if (user.BusId.HasValue) bus = AdminServices.Getbus(user.BusId.Value);
            var model = new DriverDetails()
            {
                DriverId = DriverId,
                DriverName = user.UserName,
                BusId = user.BusId,
                CurrentBus = user.BusId.HasValue ? bus.BusLoginHash : null,
                CurrentLine = user.BusId.HasValue ? bus.Line.Name : null,
                AllBuses = AdminServices.GetAllBusOptions()
            };
            return View("Drivers/EditDriver", model);
        }
        [HttpPost("/Admin/Drivers/EditDriver")]
        public IActionResult EditDriverPost(DriverInput driverInput)
        {
            var successful = AdminServices.AssignBusToDriver(driverInput.DriverId, driverInput.IdBus);
            if (successful)
                return Redirect("/Admin/Drivers/AllDrivers");
            else throw new Exception("Failed assignment");
        }
        [HttpPost("/Admin/Drivers/Dismiss")]
        public IActionResult DismissDriver(string DriverId)
        {
            bool result = AdminServices.DismissDriver(DriverId);
            if(result)
            return Redirect("/Admin/Drivers/AllDrivers");
            else throw new Exception("Failed dismissal");
        }


        [HttpGet("/Admin/Users/RoleChange")]
        public async Task<IActionResult> ChangeRole(string UserId)
        {
            string roleName = "Driver";
            bool roleExists = await RoleManager.RoleExistsAsync(roleName);
            if(!roleExists)
            {
                await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                });
            }
            var user = await UserManager.FindByIdAsync(UserId);
            var model = new UserDetails()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                CurrentRole = RoleManager.Roles.FirstOrDefault(x => x.RoleUsers.Any(x => x.UserId == UserId)).Name,
                AllRoles = RoleManager.Roles.Select(x =>x.Name)
            };
            return View("Roles/RoleChange",model);
        }
        [HttpPost("/Admin/Users/RoleChange")]
        public async Task<IActionResult> ChangeRolePost(UserInput userInput)
        {
            await UserManager.AddToRoleAsync(await UserManager.FindByIdAsync(userInput.IdUser), userInput.NewRole);
            await UserManager.RemoveFromRoleAsync(await UserManager.FindByIdAsync(userInput.IdUser), userInput.OldRole);
            return Redirect("/Admin/Roles/AllUsers");
        }
    }
}