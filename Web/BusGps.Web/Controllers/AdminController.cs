namespace BusGps.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BusGps.Data.Models;
    using BusGps.Services.Data;
    using BusGps.Web.ViewModels.Admin;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
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
            var model = UserManager.GetUsersInRoleAsync("Driver").Result.Select(x => new DriverInfo
            {
                Username = x.UserName,
                Bus = x.Bus == null ? "Not assigned" : x.Bus.BusLoginHash,
                Line = x.Bus == null ? "Not assigned" : x.Bus.Line.Name,
                Id = x.Id,
            }).ToList();
            return View("Drivers/AllDrivers", model);
        }

        [HttpGet("/Admin/Roles/AllUsers")]
        public IActionResult AllUsers()
        {
            var model = this.UserManager.Users.Select(x => new UserInfo
            {
                Username = x.UserName,
                Email = x.Email,
                Id = x.Id,
                Role = this.UserManager.GetRolesAsync(x).Result.FirstOrDefault() == null ? "User" : this.UserManager.GetRolesAsync(x).Result.FirstOrDefault(),
            }).ToList();
            return View("Roles/AllUsers", model);
        }

        [HttpGet("/Admin/Buses/AllBuses")]
        public IActionResult AllBuses()
        {
            var model = new List<BusInfo>(this.AdminServices.GetAllBuses().Select(x => new BusInfo 
            {
                BusId = x.Id.ToString(),
                BusName = x.BusLoginHash,
                Line = x.Line.Name,
                IsActive = x.IsActive.ToString(),
            }));
            return View("Buses/AllBuses", model);
        }

        [HttpGet("/Admin/Buses/CreateBus")]
        public IActionResult CreateBus()
        {
            var model = new List<LineOption>(this.AdminServices.GetLineOptions().Select(x => new LineOption
            {
                LineId = x.Id,
                LineName = x.Name,
            }));
            return View("Buses/CreateBus", model);
        }

        [HttpPost("/Admin/Buses/CreateBus")]
        public IActionResult CreateBusPost(string BusName, string LineId)
        {
            bool result = AdminServices.CreateBus(BusName, LineId).Result;
            if (result) return Redirect("/Admin/Buses/AllBuses");
            else throw new Exception("Failed Creation");
            
        }
        [HttpGet("/Admin/Buses/EditBus")]
        public IActionResult EditBus(string BusId)
        {
            var model = new EditBus
            {
                Id = this.AdminServices.Getbus(BusId).Id,
                Name = this.AdminServices.Getbus(BusId).BusLoginHash,
                LineOptions = this.AdminServices.GetLineOptions().Select(x => new LineOption
                {
                    LineId = x.Id,
                    LineName = x.Name,
                }),
            };
            return View("Buses/EditBus", model);
        }
        //[HttpPost("/Admin/Buses/EditBus")]
        //public IActionResult EditBusPost(int BusId, string BusName, int LineId)
        //{
        //    AdminServices.EditBus(BusId, BusName, LineId);
        //    return Redirect("/Admin/Buses/AllBuses");
        //}
        //[HttpPost("/Admin/Buses/DeleteBus")]
        //public IActionResult DeleteBus(int BusId)
        //{
        //    AdminServices.DeleteBus(BusId);
        //    return Redirect("/Admin/Buses/AllBuses");
        //}

        //[HttpGet("/Admin/Drivers/EditDriver")]
        //public async Task<IActionResult> EditDriver(string DriverId)
        //{
        //    var user = await UserManager.FindByIdAsync(DriverId);
        //    Bus bus = null;
        //    if (user.BusId.HasValue) bus = AdminServices.Getbus(user.BusId.Value);
        //    var model = new DriverDetails()
        //    {
        //        DriverId = DriverId,
        //        DriverName = user.UserName,
        //        BusId = user.BusId,
        //        CurrentBus = user.BusId.HasValue ? bus.BusLoginHash : null,
        //        CurrentLine = user.BusId.HasValue ? bus.Line.Name : null,
        //        AllBuses = AdminServices.GetAllBusOptions()
        //    };
        //    return View("Drivers/EditDriver", model);
        //}
        //[HttpPost("/Admin/Drivers/EditDriver")]
        //public IActionResult EditDriverPost(DriverInput driverInput)
        //{
        //    var successful = AdminServices.AssignBusToDriver(driverInput.DriverId, driverInput.IdBus);
        //    if (successful)
        //        return Redirect("/Admin/Drivers/AllDrivers");
        //    else throw new Exception("Failed assignment");
        //}
        //[HttpPost("/Admin/Drivers/Dismiss")]
        //public IActionResult DismissDriver(string DriverId)
        //{
        //    bool result = AdminServices.DismissDriver(DriverId);
        //    if(result)
        //    return Redirect("/Admin/Drivers/AllDrivers");
        //    else throw new Exception("Failed dismissal");
        //}


        [HttpGet("/Admin/Users/RoleChange")]
        public async Task<IActionResult> ChangeRole(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            var model = new UserDetails()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                CurrentRole = this.UserManager.GetRolesAsync(user).Result.FirstOrDefault() == null ? "User" : this.UserManager.GetRolesAsync(user).Result.FirstOrDefault(),
                AllRoles = this.RoleManager.Roles.Select(x => x.Name).ToArray().Append<string>("User"),
            };
            return this.View("Roles/RoleChange", model);
        }

        [HttpPost("/Admin/Users/RoleChange")]
        public async Task<IActionResult> ChangeRolePost(UserInput userInput)
        {
            if (userInput.NewRole == "User")
            {
                await this.UserManager.RemoveFromRoleAsync(await this.UserManager.FindByIdAsync(userInput.IdUser), userInput.OldRole);
            }
            else
            {
                await this.UserManager.AddToRoleAsync(await this.UserManager.FindByIdAsync(userInput.IdUser), userInput.NewRole);
                await this.UserManager.RemoveFromRoleAsync(await this.UserManager.FindByIdAsync(userInput.IdUser), userInput.OldRole);
            }

            return this.Redirect("/Admin/Roles/AllUsers");
        }
    }
}