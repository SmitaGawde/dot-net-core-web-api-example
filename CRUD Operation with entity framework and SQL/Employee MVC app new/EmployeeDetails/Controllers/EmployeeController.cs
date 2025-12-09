using EmployeeDetails.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDetails.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeServices service;
        public EmployeeController()
        {
            service = new EmployeeServices();
        }
        public async Task <IActionResult>Index()
        {
            var employee = await service.getAllEmployeeAsync ();
            return View(employee);
        }

        public IActionResult create()
        {
            return View();
        }


    }
}
    