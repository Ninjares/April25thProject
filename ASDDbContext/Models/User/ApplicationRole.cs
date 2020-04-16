using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPDbContext.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> RoleUsers { get; set; } = new HashSet<ApplicationUserRole>();
    }
}
