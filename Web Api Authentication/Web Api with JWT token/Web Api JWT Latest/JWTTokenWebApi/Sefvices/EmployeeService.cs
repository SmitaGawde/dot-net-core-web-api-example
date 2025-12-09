using JWTTokenWebApi.Context;
using JWTTokenWebApi.Interfaces;
using JWTTokenWebApi.Models;

namespace JWTTokenWebApi.Sefvices
{
    public class EmployeeService :IEmployeeService
    {
        private readonly JwtContext Jwtcontext;
        public EmployeeService(JwtContext _context)
        {
            Jwtcontext = _context;
        }

        public List<Employee> GetEmployeeDetails()
        {
            var emp = Jwtcontext.Employees.ToList();    
            return emp;
        }
        public Employee GetEmployeeDetails(int id)
        {
            var emp = Jwtcontext.Employees.SingleOrDefault(e => e.Id == id);
            return emp;
        }
        public Employee AddEmployee(Employee employee)
        {
            var emp  = Jwtcontext.Add (employee);
            Jwtcontext.SaveChanges();
            return emp.Entity;

        }
        public Employee UpdateEmployee(Employee employee)
        {
            var UpdatedEmployee = Jwtcontext.Employees.Update(employee);
            Jwtcontext.SaveChanges();
            return ( UpdatedEmployee.Entity);
        }
        public bool DeleteEmployee(int id)
        {
            try
            {
                var employee = GetEmployeeDetails(id);
                if (employee != null)
                {
                    Jwtcontext.Employees.Remove(employee);
                    Jwtcontext.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Employee not found");
                }

            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
