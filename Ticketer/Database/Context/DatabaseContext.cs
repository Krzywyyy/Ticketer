using Microsoft.EntityFrameworkCore;
using Ticketer.Database.Domain;

namespace Ticketer.Database.Context
{
    class DatabaseContext : DbContext
    {
        public DbSet<Spectacle> Spectacles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TicketReservation;Integrated Security=True");
        }
    }
}
