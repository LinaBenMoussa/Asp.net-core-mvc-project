using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TripExpress.Data;
using TripExpress.Models;

namespace TripExpress.Controllers
{
    public class DateRoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DateRoomController(ApplicationDbContext context)
        {
            _context = context;
        }
             public IActionResult EditReservationDate(int res, int room, int hotel, string user, int nbroom, string deb, string fin)
        {

            DateTime dateDeb;
            DateTime datefin;
            bool isValidDateDeb = DateTime.TryParseExact(deb, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dateDeb);
            bool isValidDateFin = DateTime.TryParseExact(fin, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out datefin);

            if (isValidDateDeb==false)
            {
                DateTime result3;
                if (DateTime.TryParseExact(deb, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result3))
                {
                    dateDeb = result3;
                }
                else { DateTime result5; if (DateTime.TryParseExact(deb, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result5)) { dateDeb = result5; } }
            }

            if (isValidDateFin==false)
            {
                DateTime result4;
                if (DateTime.TryParseExact(fin, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result4))
                {
                    datefin = result4;
                }
                else { DateTime result6; if (DateTime.TryParseExact(fin, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result6)) { datefin = result6; } }

            }

            ReservationRoom reservation = _context.ReservationRoom.Find(res);
            DateRoom date = _context.DateRooms.FirstOrDefault(d => d.IdReservationRoom == res);
            if (reservation.dateDebut!=dateDeb || reservation.dateFin != datefin || reservation.IdRoom!=room) {
                if(date!=null) {
                    _context.DateRooms.Remove(date);
                _context.SaveChanges(); }
                
                bool isAvailable = true;
                List<DateRoom> dates = _context.DateRooms.Where(d => d.idDateRoom == room).ToList();
                foreach (DateRoom d in dates)
                {
                if ((((dateDeb >= d.dateDeb && dateDeb <= d.datefin) || (datefin >= d.dateDeb && datefin <= d.datefin)) || (dateDeb <= d.dateDeb && datefin >= d.datefin))&& d.idRoom==room)
                {
                    ViewData["nonDispoMsg"] = "he room is already booked on this date";
                    isAvailable = false;
                    break;
                }
                 }
                 if (isAvailable)
                 {
                DateRoom.SupprimerDateRoomsDateDepassee(_context);

                DateRoom d = new DateRoom();
                d.dateDeb = dateDeb;
                d.datefin = datefin;
                d.idRoom = room;
                    d.IdReservationRoom = res;
                _context.DateRooms.Add(d);
                _context.SaveChanges();
                return RedirectToAction("EditReservation", "ReservationRoom", new {res=res, room = room, hotel = hotel, user = user, nbroom = nbroom, deb = deb, fin = fin });
                 }

                    else
                    {
                return RedirectToAction("editReservationForm", "ReservationRoom");
                     }
                   }
            else {
               return RedirectToAction("EditReservation", "ReservationRoom", new { res=res,room = room, hotel = hotel, user = user, nbroom = nbroom, deb = deb, fin = fin });
                 }


        }
        public IActionResult verifyReservationDate(int room, int hotel, string user, int nbroom, string deb, string fin)
        {
            DateTime.TryParseExact(deb, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateDeb);
            DateTime.TryParseExact(fin, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime datefin);
                
            bool isAvailable = true;
            List<DateRoom> dates = _context.DateRooms.Where(d => d.idRoom == room).ToList();
            foreach (DateRoom d in dates)
            {
                if (((dateDeb >= d.dateDeb && dateDeb <= d.datefin) || (datefin >= d.dateDeb && datefin <= d.datefin)) || (dateDeb <= d.dateDeb && datefin >= d.datefin))
                {
                    isAvailable = false;
                    break;

                }
                {
                    ViewData["nonDispoMsg"] = "he room is already booked on this date";
                    isAvailable = false;
                    break;
                }
            }
            if (isAvailable==true) {
                DateRoom.SupprimerDateRoomsDateDepassee(_context);

                return  RedirectToAction("Addreservation", "ReservationRoom", new { room = room, hotel = hotel, user = user, nbroom = nbroom,datedeb=deb,datefin=fin }); }

            else {
                return RedirectToAction("reservationForm", "ReservationRoom");
            }


        }
    }
}
