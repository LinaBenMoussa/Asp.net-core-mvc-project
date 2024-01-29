namespace TripExpress.Models
{
    public class hotelRoomReservation
    {
        public Hotel Hotel { get; set; } 
        public RoomModel Room { get; set; } 
        public ReservationRoom Reservation { get; set; }
    }
}
