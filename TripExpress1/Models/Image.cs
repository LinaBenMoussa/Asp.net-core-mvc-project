using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
    public class Image
    {
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey("Hotel")]
        public int idHotel{ get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
