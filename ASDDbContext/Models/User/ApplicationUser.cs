using ASPDbContext.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new HashSet<ApplicationUserRole>();
        public int? BusId { get; set; }
        public Bus Bus { get; set; }
    }
}
