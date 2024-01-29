using TripExpress.Models;

namespace TripExpress.ViewModels
{
    public class RateAvis
    {
        public string hotel { get; set; }
        public List<Comment> comment { get; set; }
        public float rate { get; set; }
    }
}
