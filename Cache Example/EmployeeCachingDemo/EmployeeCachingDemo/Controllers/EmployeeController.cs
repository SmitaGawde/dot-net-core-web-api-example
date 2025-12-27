using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using EmployeeCachingDemo.Repository;
using EmployeeCachingDemo.Models;
using Microsoft.AspNetCore.OutputCaching;


namespace EmployeeCachingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly IEmployeeRepository repository;
        public EmployeeController(IConfiguration _config,IMemoryCache _memoryCache,IDistributedCache _distributedCache,IEmployeeRepository _repository)
        {
            memoryCache = _memoryCache;
            config = _config;
            distributedCache = _distributedCache;
            repository = _repository;
        }

        [HttpGet("inMemmory")]
        public async Task<IActionResult> GetEmployeeInMemory()
        {
            const string cacheKey = "emp_inmemory";
            if (!memoryCache.TryGetValue(cacheKey,out List<Employee> employees))
            {
                employees = await repository.GetEmployeesAsync();
                memoryCache.Set(cacheKey, employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                });
            }
            return Ok(employees);
        }


        [HttpGet("distributed")]
        public async Task<IActionResult> GetEmployeesdistributed()
        {
            const string cacheyKey = "emp_distributde";
            var cachedData = await distributedCache.GetStringAsync(cacheyKey);
            List<Employee> employees;
            if (!string.IsNullOrEmpty(cachedData))
            {
                employees =JsonSerializer.Deserialize<List<Employee>> (cachedData); 
            }
            else
            {
                employees = await repository.GetEmployeesAsync();
                await distributedCache.SetStringAsync(cacheyKey, JsonSerializer.Serialize(employees), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                });
            }
            return Ok(employees);
        }

        //Response Cache
        [HttpGet("response")]
        [ResponseCache(Duration =20,Location =ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetEmployeeResponseCached()
        {
            return Ok (await repository.GetEmployeesAsync());
        }

        // Output cache
        [HttpGet("output")]
        [OutputCache(Duration =30)]
        public async Task<IActionResult> GetEmployeeOutputCache()
        {
            return Ok (await repository.GetEmployeesAsync());
        }


        // end of class

    }
}
