using BusGps.Services.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusGps.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly IMapService mapService;
        public MapController(IMapService mapService)
        {
            this.mapService = mapService;
        }
        [HttpGet("/Map/GetRoutes")]
        public async Task<JsonResult> GetRoutes()
        {
            return this.Json(mapService.GetAllRoutes());
        }
        [HttpGet("/Map/GetStops")]
        public async Task<JsonResult> GetStops()
        {
            return this.Json(mapService.GetAllStops());
        }
    }
}
