using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class DestinationController : Controller
    
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DestinationController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {

            List<Destination> objDestinationsList= _db.Destinations.ToList();
            ViewData["destinations"] = objDestinationsList;

            return View(objDestinationsList);
        }
        
        public IActionResult SearchDestination(string search,int pg = 1 )
        {
            List<Destination> objDestinationsList;

            if (!string.IsNullOrEmpty(search))
            {
                // Effectuer la recherche si le terme de recherche est fourni
                objDestinationsList = _db.Destinations
                    .Where(d => d.Nom.Contains(search) )
                    .ToList();
            }
            else
            {
                // Sinon, obtenir la liste complète
                objDestinationsList = _db.Destinations.ToList();
            }
            ViewData["search"] = search;
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objDestinationsList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = objDestinationsList.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.pager = pager;

           return View(data);
        }

        public IActionResult DestinationList(int pg=1)
        {
            List<Destination> objDestinationsList = _db.Destinations.ToList();
            const int pageSize = 5;
            if(pg < 1) { pg = 1; }
            int recsCount=objDestinationsList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data=objDestinationsList.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.pager= pager;
            return View(data);
        }
        public IActionResult RemoveDestination(int id)
        {
            Destination dest=_db.Destinations.Find(id);
            _db.Destinations.Remove(dest);
            _db.SaveChanges();
            return RedirectToAction("DestinationList");
        }
        public IActionResult AddDestinationForm()
        {
            return View();
        }
        public async Task<IActionResult> AddDestination(IFormFile img, string nom, string description, string type, string localisation)
        {

            // Check if any validation errors occurred
            if (ModelState.IsValid)
            {
                // Validate the uploaded file
                if (!img.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("img", "Invalid image format.");
                    return View();
                }

                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                // Save the image to the desired folder
                var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "destinations", fileName);

                using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                     img.CopyToAsync(fileStream);
                }

                // Create a new Destination object and set its properties
                var destination = new Destination();
                destination.Nom = nom;
                destination.Description = description;
                destination.Type = type;
                destination.localisation = localisation;
                destination.Image = fileName;

                // Add the Destination object to the database and save changes
                _db.Destinations.Add(destination);
                _db.SaveChanges();

                // Redirect to the DestinationList action
                return RedirectToAction("DestinationList");
            }else
            {
                return RedirectToAction("AddDestinationForm");
            }

          
        }

        public IActionResult EditDestinationForm(int id) 
        {
            Destination d= _db.Destinations.Find(id);   

            return View(d);
        }
        public async Task<IActionResult> EditDestination(IFormFile img, string nom, string description, string type, string localisation, int id)
        {
            // Check if the destination exists
            var destination = await _db.Destinations.FindAsync(id);
            if (destination == null)
            {
                return RedirectToAction("DestinationList");
            }

            // Validate the uploaded file
            if (img != null && !img.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("img", "Invalid image format.");
            }

            // Update destination properties
            destination.Nom = nom;
            destination.Description = description;
            destination.Type = type;
            destination.localisation = localisation;

            // Handle file upload if provided
            if (img != null)
            {
                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                // Save the image to the desired folder
                var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "destinations", fileName);

                using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                    await img.CopyToAsync(fileStream);
                }

                // Update the image path
                destination.Image = fileName;
            }

            // Update the destination in the database
            _db.Destinations.Update(destination);
            await _db.SaveChangesAsync();

            // Redirect to the DestinationList action
            return RedirectToAction("DestinationList");
        }



    }
}
