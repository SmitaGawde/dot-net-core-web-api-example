using EmployeeCachingDemo.Data;
using EmployeeCachingDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCachingDemo.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext context;
        public EmployeeRepository(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await context.employees.AsNoTracking().ToListAsync();
        }
    }
}
