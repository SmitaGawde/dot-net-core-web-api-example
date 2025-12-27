using EmployeeCachingDemo.Data;
using EmployeeCachingDemo.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

// In Memory caching

builder.Services.AddMemoryCache();

// Distributed Caching
//Sql server Distributed Caching
builder.Services.AddDistributedSqlServerCache(
    options=>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.SchemaName = "dbo";
        options.TableName = "CacheTable";
    });

// Redis Distributed Caching
builder.Services.AddStackExchangeRedisCache(options=>
{
    options.Configuration = "localhost:6379";
});

//Response caching
builder.Services.AddResponseCaching();

//Output caching
builder.Services.AddOutputCache();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();
app.UseOutputCache();

app.MapControllers();

app.Run();
