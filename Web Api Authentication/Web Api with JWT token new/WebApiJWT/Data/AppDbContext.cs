using Microsoft.EntityFrameworkCore;
using WebApiJWT.Models;

namespace WebApiJWT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options) { }

        public DbSet <User > users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity <User>().ToTable ("User");
        }

    }
}
