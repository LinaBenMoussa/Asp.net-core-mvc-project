using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripExpress.Models;

namespace TripExpress.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Image> Images { get; set; }


        public DbSet<Destination> Destinations { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public DbSet<DateRoom> DateRooms { get; set; }

        public DbSet<TripExpress.Models.ReservationRoom>? ReservationRoom { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<DateRoom>()
                .HasOne(e => e.ReservationRoom)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReservationRoom>()
            .HasOne(rr => rr.Room)
             .WithMany(r => r.ReservationRooms)
            .HasForeignKey(rr => rr.IdRoom)
            .OnDelete(DeleteBehavior.Restrict);
    }

    }
}