using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Data;
using MinimalApi.Models;
using NuGet.Protocol;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddSingleton<TokenStore>();
            //builder.Services.AddTransient<TokenHandler>();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("EmployeeDB"));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapGet ("/", ()=> "Minimal API Running Successfully");

            app.MapGet("/GetEmployeeData", async (AppDbContext dbContext) =>
            {
                return await dbContext.Employee.ToListAsync();
            });

            app.MapGet("/GetEmployeeDataById/{id}", async (int id, AppDbContext dbContext) =>
            {
                return await dbContext.Employee.FindAsync(id)
                    is Employee employee
                        ? Results.Ok(employee)
                        : Results.NotFound();
            });

            app.MapPost("/AddEmployeeData", async (Employee employee, AppDbContext dbContext) =>
            {
                dbContext.Employee.Add(employee);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/GetEmployeeDataById/{employee.EmpId}", employee);
            });

            app.MapPut("/UpdateEmployeeData/{id}", async (int id, Employee updatedEmployee, AppDbContext dbContext) =>
            {
                var employee = await dbContext.Employee.FindAsync(id);
                if (employee is null) return Results.NotFound();
                employee.Name = updatedEmployee.Name;
                employee.Position = updatedEmployee.Position;
                employee.Salary = updatedEmployee.Salary;
                employee.DateOfJoining = updatedEmployee.DateOfJoining;
                employee.DateofBirth = updatedEmployee.DateofBirth;
                employee.Email = updatedEmployee.Email;
                employee.PhoneNumber = updatedEmployee.PhoneNumber;
                employee.DeptId = updatedEmployee.DeptId;
                employee.Password = updatedEmployee.Password;
                employee.Username = updatedEmployee.Username;
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/DeleteEmployeeData/{id}", async (int id, AppDbContext dbContext) =>
            {
                if (await dbContext.Employee.FindAsync(id) is Employee employee)
                {
                    dbContext.Employee.Remove(employee);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok(employee);
                }
                return Results.NotFound();
            });

            app.MapPatch("/UpdateEmployeeSalary/{id}/{salary}", async (int id, decimal salary, AppDbContext dbContext) =>
            {
                var employee = await dbContext.Employee.FindAsync(id);
                if (employee is null) return Results.NotFound();
                employee.Salary = salary;
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
