using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private object _Context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult GetByHotel(int hotelId)
        {
            // Get rooms for the specified hotel
            List<RoomModel> rooms = _context.Rooms.Where(r=>r.IdHotel==hotelId).ToList();

            // Return the rooms
            return Ok(rooms);
        }
        public IActionResult searchRoom(string search, int pg = 1)
        {
            List<RoomModel> Rooms;

            if (!string.IsNullOrEmpty(search))
            {
                // Effectuer la recherche si le terme de recherche est fourni
                Rooms = _context.Rooms.Include(h => h.Hotel)
                    .Where(d => (d.Type.Contains(search) || d.Nbpersonne.ToString().Contains(search) || d.prixRoom.ToString().Contains(search)))
                    .ToList();
            }
            else
            {
                // Sinon, obtenir la liste complète
                Rooms = _context.Rooms.Include(h => h.Hotel).ToList();
            }
            ViewData["search"] = search;
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = Rooms.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Rooms.Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();
            this.ViewBag.pager = pager;

            return View(data);
        }
        public async Task<IActionResult> EditRoomForm(int id)
        {
            RoomModel room = _context.Rooms.Include(h => h.Hotel).FirstOrDefault(r => r.IdRoom == id);
            string nom = room.Hotel.Nom;
            ViewData["nom"] =nom;
            List<Hotel> h = _context.Hotels.ToList();
            ViewData["hotels"] = h;
            return View(room);
        }
        public async Task<IActionResult> EditRoom(int id, string type, int nbpersonne, decimal prix , string hotel)
        {
            RoomModel room=_context.Rooms.Find(id);
            room.IdHotel = _context.Hotels.FirstOrDefault(h=>h.Nom==hotel).IdHotel;
            room.Type = type;
            room.prixRoom = prix;
            room.Nbpersonne = nbpersonne;
            _context.SaveChanges();
            return RedirectToAction("ShowRoom", new { id = room.IdHotel });
        }

        public IActionResult removeRoom(int id)
        {

            RoomModel room=_context.Rooms.Find(id);
            int idh = room.IdHotel;
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return RedirectToAction("ShowRoom", new { id = room.IdHotel });
        }

        public IActionResult AddRoom(int id ,string type,int nbpersonne,decimal prix)
        {
            RoomModel room = new RoomModel();
            room.IdHotel = id;
            room.Type=type;
            room.prixRoom=prix; 
            room.Nbpersonne = nbpersonne;
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return RedirectToAction("ShowRoom", new { id = room.IdHotel });
        }
        public IActionResult AddRoomForm(int id)
        {
            ViewData["id"] = id;
            string h = _context.Hotels.FirstOrDefault(h => h.IdHotel == id).Nom;
            ViewData["name"] = h;
            return View();

        }
            public IActionResult ShowRoom(int id, int pg = 1)
        {
            ViewData["id"] = id;
            ViewData["name"] = _context.Hotels.FirstOrDefault(h=>h.IdHotel==id);  
            List<RoomModel> objRoomsList = _context.Rooms.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objRoomsList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Rooms
                .Where(r => r.IdHotel == id)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();

            this.ViewBag.pager = pager;
            return View(data);
        }

       

        private bool RoomModelExists(int id)
        {
            return (_context.Rooms?.Any(e => e.IdRoom == id)).GetValueOrDefault();
        }
        [Authorize]
        public IActionResult RoomDispo(DateTime datepicker, DateTime datepicker1, string type)
        {
            DateRoom.SupprimerDateRoomsDateDepassee(_context);

            try
            {
                int IdHotel = int.Parse(HttpContext.Request.Query["hotelId"]);
                string typeRoom = HttpContext.Request.Query["typeSelector"];
                ViewData["idHotel"] = IdHotel;
                ViewData["nomHotel"] = _context.Hotels.FirstOrDefault(h => h.IdHotel == IdHotel).Nom;
                ViewData["type"] = typeRoom;

                int nb = typeRoom == "Single" ? 1 : typeRoom == "Double" ? 2 : typeRoom == "Triple" ? 3 : 0;

                string dateIn = HttpContext.Request.Query["datepicker"];
                string dateOut = HttpContext.Request.Query["datepicker1"];

                if (DateTime.TryParseExact(dateIn, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result1) &&
                    DateTime.TryParseExact(dateOut, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result2))
                {
                    ViewData["dateDeb"] = result1;
                    ViewData["dateFin"] = result2;

                    List<RoomModel> chambresDisponibles = new List<RoomModel>();

                    foreach (RoomModel r in _context.Rooms)
                    {
                        if(r.IdHotel==IdHotel &&  nb == r.Nbpersonne) { 
                        bool isAvailable = true;

                        foreach (DateRoom d in _context.DateRooms.Where(dr => dr.idRoom == r.IdRoom))
                        {
                            if (((result1>=d.dateDeb && result1<=d.datefin)||(result2>=d.dateDeb && result2 <=d.datefin)) || (result1<=d.dateDeb && result2>=d.datefin) )
                            {
                               isAvailable=false;
                                break;
                                
                            }
                        }
                        if (isAvailable)
                        {
                            chambresDisponibles.Add(r);
                        }
                        }
                    }

                    // À ce stade, la liste chambresDisponibles contient toutes les chambres disponibles pour la période spécifiée.

                    RoomModel room = chambresDisponibles.FirstOrDefault();

                    if (room != null)
                    {
                        return View(room);
                    }
                    else
                    {
                        return View(); // ou une vue d'indisponibilité
                    }
                }
                else
                {
                    return View(); // ou une vue d'erreur
                }
            }
            catch (Exception ex)
            {
                return View("Error"); // ou une vue d'erreur
            }
        }


    }
}