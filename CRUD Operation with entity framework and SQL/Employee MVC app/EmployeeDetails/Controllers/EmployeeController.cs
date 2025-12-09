using EmployeeAdminPortal.Models.Entities;
using EmployeeDetails.Services;
using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{
    private readonly EmployeeService _service;

    public EmployeeController()
    {
        _service = new EmployeeService();
    }

    public async Task<ActionResult> Index()
    {
        var employees = await _service.getAllEmployeeAsync();
        return View(employees);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(AddemployeeDto emp)
    {
        var created = await _service.AddEmployeeAsync(emp);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Edit(Guid id)
    {
        var emp = await _service.GetEmployeeAsync(id);
        return View(emp);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Guid id, AddemployeeDto emp)
    {
        await _service.UpdateEmployeeAsync(id, emp);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(Guid id)
    {
        var emp = await _service.GetEmployeeAsync(id);
        return View(emp);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(Guid id)
    {
        await _service.DeleteEmployeeAsync(id);
        return RedirectToAction("Index");
    }
}
