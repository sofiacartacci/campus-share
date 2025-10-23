using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Models;

namespace CampusShare.Web.Context
{
    public class CampusShareDBContext : DbContext
    {
        public CampusShareDBContext(DbContextOptions<CampusShareDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
