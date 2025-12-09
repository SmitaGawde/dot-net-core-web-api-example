using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDBContext appDBContext;
        public EmployeesController(AppDBContext _appDBContext)
        {
            appDBContext= _appDBContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allemp=  appDBContext.Employees.ToList();
            return Ok (allemp);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddemployeeDto addemployeeDto)
        {
            var employeeentity = new Employee()
            { 
                Name = addemployeeDto.Name,
                Email= addemployeeDto.Email,
                Phone= addemployeeDto.Phone,
                salary=addemployeeDto.salary
            };

            appDBContext.Employees.Add(employeeentity);
            appDBContext.SaveChanges();
            return Ok(employeeentity);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult getEmployee (Guid id )
        {
            var emp =  appDBContext.Employees.Find(id);
            if (emp != null)
            {
                return Ok(emp);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult updateEmployee(Guid id,AddemployeeDto addemployeeDto)
        {
            var emp = appDBContext.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }

            emp.Name = addemployeeDto.Name;
            emp.Email = addemployeeDto.Email;
            emp.Phone = addemployeeDto.Phone;
            emp.salary = addemployeeDto.salary;
            appDBContext.SaveChanges();
            return Ok (emp);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult deleteEmployee(Guid id)
        {
            var emp = appDBContext.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }
            appDBContext.Employees.Remove(emp);

            appDBContext.SaveChanges();
            return Ok(emp);
        }

    }
}
