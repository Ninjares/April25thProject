using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Google()
        {
            return View("Google", "Map");
        }
        [HttpGet("/Map/OSM")]
        public IActionResult OSM()
        {
            string routes = OSMService.GetAllRoutes();
            var model = new OSMViewModel
            {
                Json = routes
            };
            return View(model);        
        }
    }
}