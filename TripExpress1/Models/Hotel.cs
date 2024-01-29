using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
   
        public class Hotel
        {
            [Key]
            public int IdHotel { get; set; }
        [Required]
            public string Nom { get; set; }
      
            public string? Localisation { get; set; }
        [Required]
            public string Type { get; set; }
        [Required]
            public string Img { get; set; }
        [Required]
            public int Classement { get; set; }
        public string Adresse { get; set; }


        [Required]
            public decimal PrixMin { get; set; }
        [Required]
            public decimal PrixMax { get; set; }
        public String description { get; set; }
        
        public decimal? lat { get; set; }

        public decimal? Long { get; set; }

        [ForeignKey("Destination")]
        public int IdDestination { get; set; }

        public virtual Destination Destination { get; set; }
        

    }
}
