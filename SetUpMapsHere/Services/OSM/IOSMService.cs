using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IOSMService
    {
        object GetAllRoutes(params string[] lines);
        object GetAllStops(params string[] lines);
    }
}
