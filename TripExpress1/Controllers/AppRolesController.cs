using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TripExpress.Controllers
{
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if  (!await _roleManager.RoleExistsAsync(model.Name)
)
            {
                _roleManager.CreateAsync(model).Wait();
            }
            return RedirectToAction("Index");
        }
    }
}
