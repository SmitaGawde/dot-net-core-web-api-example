using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _factory;
    public HomeController(IHttpClientFactory factory) => _factory = factory;

    public IActionResult Index() => View();

    [Authorize]
    public async Task<IActionResult> Products()
    {
        var client = _factory.CreateClient("ApiClient");
        var data = await client.GetFromJsonAsync<List<ProductDto>>("api/products");
        return View(data);
    }
}

public record ProductDto(int Id, string Name);
