using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SetUpMapsHere.Services;

namespace SetUpMapsHere.Controllers
{
    public class MapController : Controller
    {
        public IOSMService OSMService { get; set; }

        public MapController(IOSMService service)
        {
            OSMService = service;
        }
        [HttpGet("/Map/OSM")]
        public IActionResult OSM()
        {
            //var param = new string[] { };
            //object routes = OSMService.GetAllRoutes(param);
            //var model = new OSMViewModel
            //{
            //    JsonRoutes = JsonConvert.SerializeObject(routes),
            //    JsonStops = OSMService.GetAllStops(param)
            //};
            return View();
        }
        [HttpPost("/Map/GetRoutes")]
        public async Task<JsonResult> GetRoutes()
        {
            return this.Json(OSMService.GetAllRoutes());
        }
        [HttpPost("/Map/GetStops")]
        public async Task<JsonResult> GetStops()
        {
            return this.Json(OSMService.GetAllStops());
        }
    }
}