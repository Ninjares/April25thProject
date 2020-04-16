using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SetUpMapsHere.Models.Maps;
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
            var param = new string[] {  };
            string routes = OSMService.GetAllRoutes(param);
            var model = new OSMViewModel
            {
                JsonRoutes = routes,
                JsonStops = OSMService.GetAllStops(param)
            };
            return View(model);        
        }
    }
}