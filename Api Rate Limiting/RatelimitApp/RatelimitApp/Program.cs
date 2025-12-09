using Microsoft.AspNetCore.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddRateLimiter(options =>
        {
            options.OnRejected = (context, token) =>
            {
                Console.WriteLine($"❌ Rejected request at {DateTime.Now}: {context.HttpContext.Request.Path}");
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            };
            options.AddFixedWindowLimiter("fixed", c =>
            {
                c.PermitLimit = 5;
                c.Window=TimeSpan.FromSeconds(10);
                //c.QueueLimit = 5;
                //c.QueueProcessingOrder=System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            });
        }
        );

        var app = builder.Build();

        app.UseRateLimiter();

        app.MapGet("api/getdata", () =>
        {
            return "Here is the data";
        }).RequireRateLimiting("fixed");

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