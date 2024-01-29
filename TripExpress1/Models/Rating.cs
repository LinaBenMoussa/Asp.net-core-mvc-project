using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
    public class Rating
    {
        [Key]
        public int id { get; set; }
        public string? comment { get; set; }
        public int? rate { get; set; }
        public string userName { get; set; }
        [ForeignKey("Hotel")]
        public int IdHotel { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
