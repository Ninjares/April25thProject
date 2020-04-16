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
            return View("Drivers/AllDrivers",model);
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



        [HttpGet("/Admin/Drivers/CreateDriver")]
        public IActionResult CreateDriver()
        {
            return View();
        }
        [HttpGet("/Admin/Users/EditDriver")]
        public IActionResult EditDriver()
        {
            return View();
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