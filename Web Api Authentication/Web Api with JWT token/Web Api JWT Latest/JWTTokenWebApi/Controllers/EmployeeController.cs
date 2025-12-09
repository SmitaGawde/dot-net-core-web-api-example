using JWTTokenWebApi.Interfaces;
using JWTTokenWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTTokenWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public List<Employee> Get()
        {
            var emp = _employeeService.GetEmployeeDetails();
            return emp;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            var emp = _employeeService.GetEmployeeDetails(id);
            return emp;
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public Employee Post([FromBody] Employee employee)
        {
            if (employee != null)
            {
                var emp = _employeeService.AddEmployee(employee);
                return emp;
            }
            else
            {
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null");
            }
           
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public Employee Put(int id, [FromBody] Employee employee)
        {
            Employee existingEmployee = _employeeService.GetEmployeeDetails(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
            else
            {
                employee.Id = id; // Ensure the ID is set to the existing employee's ID
                var updatedEmployee = _employeeService.UpdateEmployee(employee);
                return updatedEmployee;
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var isDeleted = _employeeService.DeleteEmployee(id);
            if (!isDeleted)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found or could not be deleted.");
            }
            else
            {
                return isDeleted;
            }
        }
    }
}
