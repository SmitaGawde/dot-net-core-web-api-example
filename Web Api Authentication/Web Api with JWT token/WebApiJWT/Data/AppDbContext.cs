using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiJWT.Model;
using Microsoft.EntityFrameworkCore;

namespace WebApiJWT.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
    :   base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Product>().ToTable("Product");
        }
    }
}
