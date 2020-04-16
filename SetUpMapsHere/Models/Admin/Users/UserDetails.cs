using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Models.Admin
{
    public class UserDetails
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public string Id { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
    }
}
