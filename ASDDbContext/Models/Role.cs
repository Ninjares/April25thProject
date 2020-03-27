using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASDDbContext.Models
{
    public class Role : IdentityRole<string>
    {
        public Role():base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<User>();
        }

        public ICollection<User> Users { get; set; }
    }
}
