using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AttributeRoutingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        public static readonly List<Employee> employees = new()
        {
            new Employee {Id=1,Name="Smita",Department="IT",Salary=60000},
            new Employee{Id =2,Name="Rahul",Department="HR",Salary=45000}
        };

        // bsaic Attribute Routing
        [HttpGet]
        public IActionResult GetALL()
        {
            return Ok(employees);
        }

        // Parameter Router
        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);
            return emp == null ? NotFound() : Ok(emp);
        }

        //Named Route
       [HttpGet("details/{id:int}", Name = "GetEmployeeById")]
        public IActionResult GetEmpDetailsById(int id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);
            return emp == null ? NotFound() : Ok(emp);
        }


        //Route with constraints
        [HttpGet("salary/{salary:double}")]
        public IActionResult GetBySalary(decimal salary)
        {
            return Ok(employees.Where(e => e.Salary == salary));
        }

        //Multiple Route attributes
        [HttpGet("dept/{department}")]
        [HttpGet("department/{department}")]
        public IActionResult GetByDepartment(string department)
        {
            return Ok(employees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase)));
        }

        //Optional Parameter
        [HttpGet("search/{name?}")]
        public IActionResult Search(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return Ok(employees);
            return Ok(employees.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
        }

        // route with HTTP web + custom template
        [HttpPost("add")]
        public IActionResult AddEmployee([FromBody] Employee emp)
        {
            emp.Id = employees.Max(e => e.Id) + 1;
            employees.Add(emp);
            return CreatedAtRoute("GetEmployeeById", new { id = emp.Id }, emp);
        }

        // attribute routing with action name tokem
        [HttpGet("[action]")]
        public IActionResult Count()
        {
            return Ok(employees.Count);
        }

        // attribute routing with controller + action

        [HttpGet("[controller]/[action]")]
        public IActionResult HighestSalary()
        {
            return Ok(employees.OrderByDescending(e => e.Salary).First());
        }


        //route constraint -Range
        [HttpGet("range/{id:int:min(1):max(100)}")]
        public IActionResult GetInRange(int id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);
            return emp == null ? Ok(employees) : Ok(emp);
        }

        //Versioned Style route 
        [HttpGet("v1/employees")]
        public IActionResult GetV1()
        {
            return Ok("Employees API Version 1");
        }

        // end of class
    }
}
