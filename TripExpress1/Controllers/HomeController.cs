using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;

        }

        public IActionResult Index(string success)
        {
            if(success != null) { ViewData["Success"] = "Reservation Added"; }
            List<Destination> objDestinationsList = _db.Destinations.ToList();

            // Sélectionnez de manière aléatoire 5 objets distincts
            List<Destination> randomDestinations = objDestinationsList
                .OrderBy(x => Guid.NewGuid())
                .Distinct()
                .Take(5)
                .ToList();

            // Passez la liste aléatoire à la vue
            return View(randomDestinations);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AllDestinations()
        {
            List<Destination> objDestinationsList = _db.Destinations.ToList();

            ViewData["destination"] = objDestinationsList;
            return View("../Views/Hotel/Index.cshtml");
        }
    }

}