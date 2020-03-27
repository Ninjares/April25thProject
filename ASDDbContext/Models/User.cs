using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASDDbContext.Models
{
    public class User: IdentityUser<string>
    {
        public User() : base()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
