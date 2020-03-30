using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MakerTracker.Models;

namespace MakerTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MakerTrackerContext _db;

        public HomeController(ILogger<HomeController> logger, MakerTrackerContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var email = User.Identity.Name;
            var profile = _db.Profiles.FirstOrDefault(x => x.Email == email);
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
