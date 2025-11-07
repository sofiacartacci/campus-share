using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CampusShare.Web.Context
{
    public class CampusShareDBContextFactory : IDesignTimeDbContextFactory<CampusShareDBContext>
    {
        public CampusShareDBContext CreateDbContext(string[]? args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CampusShareDBContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CampusShareDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new CampusShareDBContext(optionsBuilder.Options);
        }
    }
}
