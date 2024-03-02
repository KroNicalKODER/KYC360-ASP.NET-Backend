using KYC360.Models;
using Microsoft.EntityFrameworkCore;

namespace KYC360.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Name> Names { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
