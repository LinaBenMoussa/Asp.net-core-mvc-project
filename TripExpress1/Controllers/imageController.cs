using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class imageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public imageController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public IActionResult showImages(int pg = 1)
        {
            List<Image> images = _context.Images.Include(h => h.Hotel).ToList();

            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = images.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Images
       .Include(h => h.Hotel)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();
            this.ViewBag.pager = pager;
            return View(data);
        }
        public IActionResult addImageForm()
        {
            List<Hotel> hotels = _context.Hotels.ToList();
            return View(hotels);
        }
        public IActionResult addImage(IFormFile img, string hotel)
        {
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
                var image = new Image();
                image.idHotel = _context.Hotels.FirstOrDefault(h => h.Nom == hotel).IdHotel;
                image.name = fileName;


                // Add the Destination object to the database and save changes
                _context.Images.Add(image);
                _context.SaveChanges();

                // Redirect to the DestinationList action
                return RedirectToAction("ShowImages");
            }
            else
            {
                return RedirectToAction("addImageForm");
            }

        }
        public IActionResult remove(int id)
        {
            Image img = _context.Images.Find(id);
            _context.Images.Remove(img);
            _context.SaveChanges();
            return RedirectToAction("ShowImages");
        }
    }
}
