using JWTRefreshToken.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTRefreshToken.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDBContext()
        {
        }
        public DbSet<Models.Employee> employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Models.Employee>().ToTable("Employee");
            modelBuilder.Entity<Employee>()
            .HasOne(e => e.department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DeptId)   // Explicitly tell EF Core
            .HasConstraintName("FK_Employee_Department");
        }
      
    }
}
