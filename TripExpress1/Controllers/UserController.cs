using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
namespace TripExpress.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ListUser()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var usersWithUserRole = allUsers.Where(u => _userManager.IsInRoleAsync(u, "User").Result).ToList();
            return View(usersWithUserRole);
        }
    }
}
