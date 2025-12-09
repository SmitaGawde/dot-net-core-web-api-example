using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7161/"); // ✅ change port if different

        for (int i = 1; i <= 10; i++)
        {
            var response = await client.GetAsync("api/getdata");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request {i}: ✅ {response.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Request {i}: ❌ {response.StatusCode}");
            }

            await Task.Delay(200); // small delay (200ms) so all 10 requests fall within 10 sec
        }
    }
}