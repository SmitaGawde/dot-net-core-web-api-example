using EmployeeCachingDemo.Models;

namespace EmployeeCachingDemo.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesAsync();
    }
}
