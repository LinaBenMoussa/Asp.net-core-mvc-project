// Controller/AdminController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TripExpress.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;

namespace TripExpress.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                
                ViewData["user"] = user;
            }
            int users = _userManager.Users.Count();
            int destination=_context.Destinations.Count();
            int hotels=_context.Hotels.Count();
            int rooms=_context.Rooms.Count();
            int avis=_context.Comments.Count();
            ViewData["users"] = users;
            ViewData["destination"] = destination;
            ViewData["hotels"] = hotels;
            ViewData["rooms"] = rooms;
            ViewData["avis"] = avis;


            return View();

        }
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = _userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                    return RedirectToAction("ListAdmins");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // En cas d'erreur, revenir à la vue d'inscription de l'admin
            return View(model);
        }

        public async Task<IActionResult> ListAdmins()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var usersWithUserRole = allUsers.Where(u => _userManager.IsInRoleAsync(u, "Admin").Result).ToList();
            return View(usersWithUserRole);
        }
       
         public async Task<IActionResult> remove(string id)
        {
            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
            {
                // Gérer le cas où l'administrateur n'est pas trouvé
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(admin);

            if (result.Succeeded)
            {
                // La suppression a réussi
                return RedirectToAction("ListAdmins");
            }
            else
            {
                // La suppression a échoué, vous pouvez inspecter les erreurs dans result.Errors
                // Redirigez vers une action de gestion d'erreur ou effectuez d'autres actions nécessaires
                return RedirectToAction("ListAdmins");
            }
        }
    }
    
}
