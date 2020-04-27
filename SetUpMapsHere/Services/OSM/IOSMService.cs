using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public interface IOSMService
    {
        string GetAllRoutes(params string[] lines);
        string GetAllStops(params string[] lines);
    }
}
