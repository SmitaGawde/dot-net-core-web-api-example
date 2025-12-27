using EmployeeCachingDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCachingDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }


        public DbSet<Employee> employees => Set<Employee>();
    }
}
