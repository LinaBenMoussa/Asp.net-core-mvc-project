using System.ComponentModel.DataAnnotations;

namespace TripExpress.Models
{
    public class Destination
    {
        [Key]
        public int IdDestination { get; set; } 
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public String localisation { get; set; }
        public String Image { get; set; }



    }
}
