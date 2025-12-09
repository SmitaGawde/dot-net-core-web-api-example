using Microsoft.EntityFrameworkCore;

namespace JWTTokenWebApi.Context
{
    public class JwtContext:DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {
        }
        public DbSet<Models.user> Users { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.user>().ToTable("Users");
            modelBuilder.Entity<Models.Employee>().ToTable("Employees");
        }
        
            
        
    }
}
