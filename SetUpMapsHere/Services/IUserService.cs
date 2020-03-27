using ASDDbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IUserService
    {
        public User GetUser(string name, string password);
        public bool RegisterUser(string name, string email, string password);
            
    }
}
