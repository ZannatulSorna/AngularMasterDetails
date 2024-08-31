using Microsoft.EntityFrameworkCore;

namespace AngularMasterDetails.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public virtual DbSet<Country>  Countries { get; set; }
        public virtual DbSet<City>   Cities { get; set; }
        
    }
}
