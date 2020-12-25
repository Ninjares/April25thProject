using System;
using System.Collections.Generic;
using System.Text;

namespace BusGps.Services.Data
{
    public interface IMapService
    {
        object GetAllRoutes(params string[] lines);
        object GetAllStops(params string[] lines);
    }
}
