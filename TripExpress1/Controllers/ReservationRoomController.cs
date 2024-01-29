using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class ReservationRoomController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReservationRoomController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        
        public async Task<IActionResult> EditReservation(int res, int room, int hotel,String  user, int nbroom, string deb, string fin)
        {
            ReservationRoom reservation = _context.ReservationRoom.Find(res);
            DateRoom date = new DateRoom();

            IdentityUser u = await _userManager.FindByEmailAsync(user);


            DateTime dateDeb;
            DateTime datefin;
            bool isValidDateDeb = DateTime.TryParseExact(deb, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dateDeb);
            bool isValidDateFin = DateTime.TryParseExact(fin, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out datefin);

            if (isValidDateDeb == false)
            {
                DateTime result3;
                if (DateTime.TryParseExact(deb, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result3))
                {
                    dateDeb = result3;
                }
                else { DateTime result5; if (DateTime.TryParseExact(deb, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result5)) { dateDeb = result5; } }
            }

            if (isValidDateFin == false)
            {
                DateTime result4;
                if (DateTime.TryParseExact(fin, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result4))
                {
                    datefin = result4;
                }
                else { DateTime result6; if (DateTime.TryParseExact(fin, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result6)) { datefin = result6; } }

            }
            reservation.dateDebut = dateDeb;
            reservation.dateFin = datefin;
           
            reservation.UserName = u.UserName;
            reservation.nbRoom = nbroom;
            reservation.PhoneNumber = u.PhoneNumber;
            reservation.Date = DateTime.Now;
            reservation.IdRoom = room;
            reservation.IdHotel = hotel;
            _context.ReservationRoom.Update(reservation);
            _context.SaveChanges();
            return RedirectToAction("ReservationRoomList");
        }

        public async Task<IActionResult> editReservationForm(int id)
        {
            ReservationRoom res=_context.ReservationRoom.Find(id);
            List<IdentityUser> user =  _userManager.Users.ToList();
            List<Hotel> hotel = _context.Hotels.ToList();
            List<RoomModel>  room = _context.Rooms.ToList();
            ViewData["hotels"] = hotel;
            ViewData["rooms"] = room;
            ViewData["res"] = res;
            return View(user);
        }
        public IActionResult remove(int id)
        {
            ReservationRoom res = _context.ReservationRoom.FirstOrDefault(r => r.IdReservationRoom == id);

            DateRoom date= _context.DateRooms.FirstOrDefault(r => r.IdReservationRoom==id);
            if (date != null) {
                _context.DateRooms.Remove(date);
            _context.SaveChanges(); }
           

            _context.ReservationRoom.Remove(res);
            _context.SaveChanges();
            return RedirectToAction("ReservationRoomList");
        }
        
            [HttpGet]
        public IActionResult reservationForm()
        {
            List<IdentityUser>  users = _userManager.Users.ToList();
            List<Hotel> hotels=_context.Hotels.ToList();
            List<RoomModel> rooms = _context.Rooms.ToList();
            ViewData["hotels"] = hotels;
            ViewData["rooms"] = rooms;
            
            return View(users);
        }
        //[HttpPost]
        public async Task<IActionResult> Addreservation(int room,int hotel,string user,int nbroom,string datedeb,string datefin)
        {
            ReservationRoom res=new ReservationRoom();
            DateRoom date=new DateRoom();
            
            IdentityUser u = await _userManager.FindByEmailAsync(user);
            string dateOut = HttpContext.Request.Query["dateFin"];
            DateTime.TryParseExact(datedeb, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result1);
            DateTime.TryParseExact(datefin, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result2);
            res.dateDebut= result1;
            res.dateFin= result2;
           
            res.UserName = u.UserName;
            res.nbRoom= nbroom;
            res.PhoneNumber = u.PhoneNumber;
            res.Date= DateTime.Now;
            res.IdRoom = room;
            res.IdHotel = hotel;
            _context.ReservationRoom.Add(res);
           _context.SaveChanges();
            date.datefin = result2;
            date.dateDeb = result1;
            date.idRoom = room;
            date.IdReservationRoom = res.IdReservationRoom;
            _context.DateRooms.Add(date);
            _context.SaveChanges();
            return RedirectToAction("ReservationRoomList");

        }
        public IActionResult ShowReservationByRoom(int id, int pg =  1)

        {
            List<ReservationRoom> objreservationList = _context.ReservationRoom.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objreservationList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.ReservationRoom.Where(r => r.IdRoom == id)
       .Include(h => h.Room).Include(h => h.Hotel)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();

            this.ViewBag.pager = pager;
            return View(data);
        }



        [Authorize]
        public async Task<IActionResult> UserReservation(int pg = 1)
        {
            IdentityUser utilisateurActuel = await _userManager.GetUserAsync(User);
            string id = utilisateurActuel.UserName;

            List<ReservationRoom> Rroom = _context.ReservationRoom.Where(r => r.UserName == id).ToList();

            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = Rroom.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = _context.ReservationRoom
                .Where(r => r.UserName == id)
                .Include(r => r.Room)  // Utilisez la propriété de navigation Room plutôt que d'accéder directement à la clé étrangère IdRoom
                    .ThenInclude(room => room.Hotel)  // Utilisez la propriété de navigation Hotel à l'intérieur de la propriété de navigation Room
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToList();

            this.ViewBag.pager = pager;

            return View(data);
        }

        public IActionResult Index(int idRoom,int idHotel,int nbRoom)
        {

            string dateIn = HttpContext.Request.Query["dateDeb"];
            string dateOut = HttpContext.Request.Query["dateFin"];
            DateTime.TryParseExact(dateIn, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime result1);
                DateTime.TryParseExact(dateOut, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime result2);
            ReservationRoom Rroom = new ReservationRoom();

            Rroom.Date = DateTime.Now;
            Rroom.IdHotel = idHotel;
            Rroom.IdRoom = idRoom;
            Rroom.nbRoom = nbRoom;
            Rroom.UserName = User.Identity.Name;
            Rroom.dateDebut = result1;
            Rroom.dateFin = result2;
            string userId = _userManager.GetUserId(User);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;
            Rroom.PhoneNumber = user.PhoneNumber;
            
            _context.ReservationRoom.Add(Rroom);
            _context.SaveChanges();
            RoomModel room=_context.Rooms.FirstOrDefault(r=>r.IdRoom==idRoom);
            DateRoom dateRoom = new DateRoom();
            dateRoom.IdReservationRoom = Rroom.IdReservationRoom;
            dateRoom.dateDeb = result1;
            dateRoom.datefin = result2;
            dateRoom.idRoom = idRoom;
            _context.DateRooms.Add(dateRoom);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Reservation Added";
            ViewData["Success"] = TempData["SuccessMessage"];
            return RedirectToAction("Index", "Home", new {success= "Reservation Added" });
        }
        public IActionResult ReservationRoomList(int pg = 1)
        {
            List<ReservationRoom> objreservationList = _context.ReservationRoom.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = objreservationList.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.ReservationRoom
       .Include(h=>h.Room).Include(h => h.Hotel)
       .Skip(recSkip)
       .Take(pager.PageSize)
       .ToList();


            this.ViewBag.pager = pager;
            return View(data);
        }

    }
    }
