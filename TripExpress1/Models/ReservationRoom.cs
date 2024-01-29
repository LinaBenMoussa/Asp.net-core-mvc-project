using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
    public class ReservationRoom
    {
        [Key]
        public int IdReservationRoom { get; set; }

        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public int nbRoom { get; set; }

        public int IdHotel { get; set; }
        public int IdRoom { get; set; }
        public DateTime? dateDebut { get; set; }
        public DateTime? dateFin { get; set; }

        [ForeignKey("IdHotel")]
        public virtual Hotel Hotel { get; set; }

        [ForeignKey("IdRoom")]
        public virtual RoomModel Room { get; set; }
    }
}
