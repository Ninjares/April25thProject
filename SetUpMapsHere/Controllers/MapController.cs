using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SetUpMapsHere.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Google()
        {
            return View("Google", "Map");
        }

        public IActionResult OSM()
        {
            return View("OSM", "Map");        
        }
    }
}