using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public RatingController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
      
       
        [Authorize]
        public async Task<IActionResult> rating(int id, int rating)
        {
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // Gérer le cas où l'utilisateur actuel est null
                return RedirectToAction("Error");
            }
            var name = currentUser.UserName;

            Rating Rating = await _context.Rating.FirstOrDefaultAsync(a => (a.userName == name && a.IdHotel == id));

            if (Rating != null)
            {
                // Assurez-vous que les propriétés nécessaires ne sont pas null
                if (Rating.rate != null)
                {
                    Rating.rate = rating;
                    _context.Rating.Update(Rating);
                }
            }
            else
            {
                Rating newAvis = new Rating
                {
                    IdHotel = id,
                    rate = rating,
                    userName = name
                };
                _context.Rating.Add(newAvis);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("HotelDescription","Hotels", new { id = id });
        }

    }
}
