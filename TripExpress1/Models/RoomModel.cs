using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
    public class RoomModel
    {
        [Key]
        public int IdRoom { get; set; }

        
        public String Type { get; set; }

        [Required]
        public Decimal prixRoom { get; set; }

        [Required]
        public int Nbpersonne { get; set; }
        public int IdHotel { get; set; }

        [ForeignKey("IdHotel")]
        public virtual Hotel Hotel { get; set; }

        public ICollection<ReservationRoom> ReservationRooms { get; set; }

       public String ToString()
        {
            return this.Type +"|"+ this.Nbpersonne + "|" + this.prixRoom;
        }
    }
}
