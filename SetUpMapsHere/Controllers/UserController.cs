using ASDDbContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SetUpMapsHere.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Controllers
{
    public class UserController : Controller
    {
        public IUserService UserService { get; set; }
        public SignInManager<User> SignInManager { get; set; }
        public UserManager<User> UserManager { get; set; }
        public UserController(IUserService service, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.UserService = service;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }
        [HttpGet("/User/Login")]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpGet("/User/Register")]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost("/User/Login")]
        public async Task<IActionResult> Login(Models.User.Login login)
        {
            var user = UserService.GetUser(login.Username, login.Password);
            if (user == null) return Redirect("/User/Login");
            else
            {
                await SignInManager.SignInAsync(user, true);
                return Redirect("/");
            }
        }
        [HttpPost("/User/Register")]
        public IActionResult Register(Models.User.Register register)
        {
            if (register.Password == register.ConfirmPassword)
            {
                bool success = UserService.RegisterUser(register.Username, register.Email, register.Password);
                if (success) return Redirect("/");
                else return Redirect("/User/Register");
            }
            else return Redirect("/User/Register");
        }

        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
