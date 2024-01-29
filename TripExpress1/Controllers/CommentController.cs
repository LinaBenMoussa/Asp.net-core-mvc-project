using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public CommentController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        public async Task<IActionResult> addComment(int id, string comment)
        {
            IdentityUser currentUser = await _userManager.GetUserAsync(User);
            var name = currentUser.UserName;

            Comment cmnt=new Comment();
            cmnt.UserName = name;
            cmnt.comment = comment;
            cmnt.IdHotel = id;
            _context.Comments.Add(cmnt);
            await _context.SaveChangesAsync();
            return RedirectToAction("HotelDescription", "Hotels", new { id = id });

        }
    }
}
