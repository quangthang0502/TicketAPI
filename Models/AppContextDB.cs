using Microsoft.EntityFrameworkCore;

namespace TicketAPI.Models
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> opt) : base(opt)
        {

        }

        public DbSet<Users> user { get; set; }
        public DbSet<Events> events { get; set; }
        public DbSet<Sold> sold { get; set; }
        public DbSet<TicketType> ticket_type { get; set; }
        public DbSet<UserReview> user_review { get; set; }
    }
}
