using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripExpress.Models
{
    public class Comment
    {
        [Key]
        public int idComment { get; set; }
        public String? UserName { get; set; }
        public String? comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Hotel")]
        public int IdHotel { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
