using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDBContext appDBContext;
        public EmployeeController(AppDBContext _appDBContext) 
        {
            appDBContext = _appDBContext;
        }

        [HttpGet]
        public IActionResult getAllEmployee ()
        {
            var emp = appDBContext.EmployeeDetail.ToList();
            return Ok (emp);
        }

        [HttpGet]
        [Route("id:int")]
        public IActionResult getEmployee(int id)
        {
            var emp = appDBContext.EmployeeDetail.Find(id);
            if (emp != null)
            {
                return Ok (emp);
            }
            else
            {
                return NotFound();
            }                
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto employeeDto)
        {
            var emp = new EmployeeDetails
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };
            appDBContext.Add(emp);
            appDBContext.SaveChanges();
            return Ok(emp);
        }

        [HttpPut]
        [Route("id:int")]
        public IActionResult EditEmployee(EmployeeDto employeeDto, int id)
        {
            var emp = appDBContext.EmployeeDetail.Find(id);

            emp.Name = employeeDto.Name;
            emp.Email = employeeDto.Email;
            emp.Phone = employeeDto.Phone;
            emp.Salary = employeeDto.Salary;
            
            appDBContext.Update(emp);
            appDBContext.SaveChanges();
            return Ok(emp);
        }

        [HttpDelete]
        [Route("id:int")]
        public IActionResult DeleteEmployee(EmployeeDto employeeDto, int id)
        {
            var emp = appDBContext.EmployeeDetail.Find(id);
            if (emp == null )
            {
                return NotFound(); 
            }
            appDBContext.Remove(emp);
            appDBContext.SaveChanges();
            return Ok(emp);
        }

    }
}
