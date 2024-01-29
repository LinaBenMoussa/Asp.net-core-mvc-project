using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;
using TripExpress.ViewModels;

namespace TripExpress.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public HotelsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]

       
        public IActionResult AddHotelForm()
        {
            List<Destination> destinationList = _context.Destinations.ToList();
            return View(destinationList);
        }
        public IActionResult AddHotelInDestinationForm(int id)
        {
            string destination = _context.Destinations.FirstOrDefault(d => d.IdDestination == id).Nom;
            ViewData["nom"] = destination;
            return View();
        }
        public async Task<IActionResult> AddHotel(IFormFile img, string nom, string type, int classement, string destination, string adresse, decimal prixMin, decimal prixMax)
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
                var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "hotels", fileName);

                using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                    img.CopyToAsync(fileStream);
                }

                // Create a new Destination object and set its properties
                var hotel = new Hotel();
                hotel.Nom = nom;
                int id = _context.Destinations.FirstOrDefault(d => d.Nom == destination).IdDestination;
                hotel.IdDestination = id;
                hotel.Type = type;
                hotel.Classement = classement;
                hotel.Img = fileName;
                hotel.Adresse = adresse;

                // Add the Destination object to the database and save changes
                _context.Hotels.Add(hotel);
                _context.SaveChanges();

                // Redirect to the DestinationList action
                return RedirectToAction("HotelsList");
            }
            else
            {
                return RedirectToAction("AddHotelForm");
            }


        }
        public IActionResult SearchHotel(string search, int pg = 1)
        {
            List<Hotel> hotels;

            if (!string.IsNullOrEmpty(search))
            {
                // Effectuer la recherche si le terme de recherche est fourni
                hotels = _context.Hotels.Include(h => h.Destination)
                    .Where(d => (d.Nom.Contains(search) || d.Classement.ToString().Contains(search) || d.Type.Contains(search)))
                    .ToList();
            }
            else
            {
                // Sinon, obtenir la liste complète
                hotels = _context.Hotels.Include(h => h.Destination).ToList();
            }
            ViewData["search"] = search;
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = hotels.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = hotels.Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();
            this.ViewBag.pager = pager;

            return View(data);
        }

        public async Task<IActionResult> EditHotel1(IFormFile image, string nom, string localisation, string type, int classement, string adresse, decimal prixMin, decimal prixMax, int id)
        {
            // Check if the destination exists
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return RedirectToAction("HotelsList");
            }

            // Validate the uploaded file
            if (image != null && !image.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("img", "Invalid image format.");
            }

            // Update destination properties
            hotel.Nom = nom;
            hotel.Adresse = adresse;
            hotel.Type = type;
            hotel.Localisation = localisation;
            hotel.Classement = classement;
            hotel.PrixMax = prixMax;
            hotel.PrixMin = prixMin;
            // Handle file upload if provided
            if (image != null)
            {
                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                // Save the image to the desired folder
                var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "hotels", fileName);

                using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Update the image path
                hotel.Img = fileName;
            }

            // Update the destination in the database
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();

            // Redirect to the DestinationList action
            return RedirectToAction("HotelsList");
        }


        public async Task<IActionResult> EditHotel(IFormFile image, string nom, string localisation, string type, int classement, string adresse, decimal prixMin, decimal prixMax, int id)
        {
            // Check if the destination exists
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return RedirectToAction("HotelsList");
            }

            // Validate the uploaded file
            if (image != null && !image.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("img", "Invalid image format.");
            }

            // Update destination properties
            hotel.Nom = nom;
            hotel.Adresse = adresse;
            hotel.Type = type;
            hotel.Localisation = localisation;
            hotel.Classement = classement;
            hotel.PrixMax = prixMax;
            hotel.PrixMin = prixMin;
            // Handle file upload if provided
            if (image != null)
            {
                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                // Save the image to the desired folder
                var destinationPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "hotels", fileName);

                using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Update the image path
                hotel.Img = fileName;
            }

            // Update the destination in the database
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();

            // Redirect to the DestinationList action
            return RedirectToAction("findHotelByDestination", new { id = hotel.IdDestination });
        }

        public IActionResult findHotelByDestination(int id, int pg = 1)
        {
            List<Hotel> objHotelsList = _context.Hotels.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objHotelsList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            List<Hotel> data = _context.Hotels
        .Include(h => h.Destination).Where(h => h.IdDestination == id)
        .Skip(recSkip)
        .Take(pager.PageSize)
        .ToList();
            ViewData["id"] = id;
            ViewData["nom"] = _context.Destinations.FirstOrDefault(d => d.IdDestination == id).Nom;
            this.ViewBag.pager = pager;
            return View(data);
        }
        [HttpGet]

        public IActionResult showRateAvis(int id)
        {
            String hotel = _context.Hotels.Find(id).Nom;
            RateAvis ra= new RateAvis();
            List<Rating> rates = _context.Rating.Where(r => r.IdHotel == id).ToList();
            List<Comment> comments=_context.Comments.Where(c=>c.IdHotel==id).ToList();
            float M=0;
            foreach(Rating r in rates)
            {
                M = M+ (float)r.rate;
            }
            M = M/rates.Count;
            ra.rate = M;
            ra.comment = comments;
            ra.hotel= hotel;
            return View(ra);
        }
        

            public IActionResult HotelsList(int pg = 1)
        {
            List<Hotel> objHotelsList = _context.Hotels.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objHotelsList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Hotels
       .Include(h => h.Destination)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();
            this.ViewBag.pager = pager;
            return View(data);
        }
        public IActionResult EditHotelForm1(int id)
        {
            Hotel h = _context.Hotels.Find(id);
            return View(h);
        }




        public IActionResult EditHotelForm(int id)
        {
            Hotel h = _context.Hotels.Find(id);
            return View(h);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            Hotel h = _context.Hotels.Find(id);

            return View(h);
        }




        private bool HotelExists(int id)
        {
            return (_context.Hotels?.Any(e => e.IdHotel == id)).GetValueOrDefault();
        }
        public IActionResult destinationHotel()
        {
            destinationHotelViewData DH = new destinationHotelViewData();
            DH.Destinations = _context.Destinations.ToList();
            DH.Hotels = _context.Hotels.ToList();



            string selectedPriceRange1 = Request.Query["priceRange1"];
            string selectedCountry = Request.Query["pays"];
            string selectedPriceRange = Request.Query["priceRange"];

            if (selectedCountry != "Select Destination" && !string.IsNullOrEmpty(selectedPriceRange) && !string.IsNullOrEmpty(selectedPriceRange1))
            {

                string[] parties = selectedPriceRange.Split('-');
                string[] parties1 = selectedPriceRange1.Split('-');
                DH.Hotels = _context.Hotels.Where(h => (h.PrixMin >= decimal.Parse(parties[0])) && (h.PrixMin <= decimal.Parse(parties[1])) && (h.Destination.Nom == selectedCountry) && (h.PrixMax >= decimal.Parse(parties1[0])) && (h.PrixMax <= decimal.Parse(parties1[1]))).ToList();

            }
            else if (!string.IsNullOrEmpty(selectedPriceRange) && !string.IsNullOrEmpty(selectedPriceRange1))
            {
                string[] parties = selectedPriceRange.Split('-');
                string[] parties1 = selectedPriceRange1.Split('-');
                DH.Hotels = _context.Hotels.Where(h => (h.PrixMin >= decimal.Parse(parties[0])) && (h.PrixMin <= decimal.Parse(parties[1])) && (h.PrixMax >= decimal.Parse(parties1[0])) && (h.PrixMax <= decimal.Parse(parties1[1]))).ToList();
            }
            else if (selectedCountry != "Select Destination" && !string.IsNullOrEmpty(selectedPriceRange))
            {
                string[] parties = selectedPriceRange.Split('-');
                DH.Hotels = _context.Hotels.Where(h => h.Destination.Nom == selectedCountry && h.PrixMin >= decimal.Parse(parties[0]) && h.PrixMin <= decimal.Parse(parties[1])).ToList();

            }
            else if (selectedCountry != "Select Destination" && !string.IsNullOrEmpty(selectedPriceRange1))
            {
                string[] parties1 = selectedPriceRange1.Split('-');
                DH.Hotels = _context.Hotels.Where(h => h.Destination.Nom == selectedCountry && h.PrixMax >= decimal.Parse(parties1[0]) && h.PrixMax <= decimal.Parse(parties1[1])).ToList();

            }
            else if (selectedCountry != "Select Destination")
            {
                DH.Hotels = _context.Hotels.Where(h => h.Destination.Nom == selectedCountry).ToList();

            }
            else if (!string.IsNullOrEmpty(selectedPriceRange1))
            {
                string[] parties1 = selectedPriceRange1.Split('-');
                DH.Hotels = _context.Hotels.Where(h => h.PrixMax >= decimal.Parse(parties1[0]) && h.PrixMax <= decimal.Parse(parties1[1])).ToList();

            }
            else if (!string.IsNullOrEmpty(selectedPriceRange))
            {
                string[] parties = selectedPriceRange.Split('-');
                DH.Hotels = _context.Hotels.Where(h => h.PrixMin >= decimal.Parse(parties[0]) && h.PrixMin <= decimal.Parse(parties[1])).ToList();

            }
            return View(DH);
        }
        public IActionResult RemoveHotel(int id)
        {
            Hotel dest = _context.Hotels.Find(id);
            _context.Hotels.Remove(dest);
            _context.SaveChanges();
            return RedirectToAction("HotelsList");
        }




        public IActionResult HotelDescription(int id)
        {
            int i = 0;
            float R = 0;
            List<Rating> rates= _context.Rating.Where(i=>i.IdHotel==id).ToList();
            foreach(var r in rates)
            {
                i++;
                R = (float)(R + r.rate);
            }
            var M = R / i;
            if(rates.Count > 0) {ViewData["rating"] = M; } else { ViewData["rating"] = 0; }
            
            List<Comment> comments = _context.Comments.Where(c=>c.IdHotel==id).ToList();
            ViewData["comment"]= comments;
            Hotel h = _context.Hotels.Find(id);
           
            List<Image> images = _context.Images.Where(i=>i.idHotel==id).ToList();

            List<Image> images1 = images.Take(2).ToList();
            List<Image> images2 = images.Skip(2).Take(1).ToList();
            List<Image> images3 = images.Skip(3).Take(1).ToList();
            List<Image> image4 = images.Skip(4).Take(2).ToList();
            ViewData["image1"]= images1;
            ViewData["image2"] = images2;
            ViewData["image3"]=images3;
            ViewData["image4"] = image4;
            return View(h);
        }
    }
}
    
