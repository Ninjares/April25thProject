using SetUpMapsHere.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IAdminService
    {
        public IEnumerable<BusInfo> GetAllBuses();
    }
}
