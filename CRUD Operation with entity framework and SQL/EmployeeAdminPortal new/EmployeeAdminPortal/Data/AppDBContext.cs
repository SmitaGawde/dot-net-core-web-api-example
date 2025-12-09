using EmployeeAdminPortal.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet <EmployeeDetails>  EmployeeDetail { get; set; }
    }
}
