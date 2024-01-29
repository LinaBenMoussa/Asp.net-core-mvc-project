using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class avisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public avisController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AvisList(int pg = 1)
        {
            List<Comment> comment = _context.Comments.ToList();

            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = comment.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Comments
      .Include(h=>h.Hotel)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();
            this.ViewBag.pager = pager;
            return View(data);
        }
        public IActionResult remove(int id)
        {
            Comment cmnt = _context.Comments.Find(id);
            _context.Comments.Remove(cmnt);
            _context.SaveChanges();
            return RedirectToAction("AvisList");
        }

    }
}
