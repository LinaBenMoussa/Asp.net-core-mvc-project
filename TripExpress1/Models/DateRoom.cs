using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripExpress.Data;

namespace TripExpress.Models
{
    public class DateRoom
    {
       
        [Key]
        public int idDateRoom { get; set; }
        public DateTime dateDeb { get; set; }
        public DateTime datefin { get; set; }
        public int idRoom { get; set; }
        [ForeignKey("idRoom")]
        public virtual RoomModel Room { get; set; }
        public int IdReservationRoom { get; set; }
        [ForeignKey("IdReservationRoom")]
        public virtual ReservationRoom ReservationRoom { get; set; }
        public  static void SupprimerDateRoomsDateDepassee(ApplicationDbContext context)
        {
            var dateActuelle = DateTime.Now;

            // Récupère le contexte de la base de données (à injecter dans le constructeur)

            // Récupère les DateRoom avec une date de fin dépassée
            var dateRoomsADelete = context.DateRooms
                .Where(dr => dr.datefin < dateActuelle)
                .ToList();

            // Supprime les DateRoom de la base de données
            context.DateRooms.RemoveRange(dateRoomsADelete);
            context.SaveChanges();

        }
        

    }
}
