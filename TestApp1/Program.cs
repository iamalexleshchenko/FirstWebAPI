using Microsoft.EntityFrameworkCore;
using TestApp1.Database;
using TestApp1.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        // создание builder
        var builder = WebApplication.CreateBuilder(args);
        
        // конфигурация сервисов builder
        ConfigureServices(builder.Services);
        
        // создание объекта веб-приложения
        var app = builder.Build();
        
        // конфигурация middleware
        ConfigureMiddleware(app);
        
        // добавление minimalApi (аналог контроллеров)
        AddMinimalApi(app);
        
        // асинхронный запуск объекта веб-приложения
        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<DatabaseContext>((options) =>  
        {
            options.UseSqlite("Data Source=test.db");
        });
        
        services.AddTransient<TestService>();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.MapControllers();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
    }

    private static void AddMinimalApi(WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        app.MapGet("/weatherforecast", (string test, string text) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)],
                            TestField = test,
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        app.MapPost("/huinya", (PostTest test) =>
        {
            return $"Idi na hui, {test.Name}";
        });

        app.MapPost("/lolz", (TestBody text) =>
        {
            string result = "";
            for (; text.Item > 0; text.Item--)
            {
                result += text.LOlz.LolzIteration(text.LOlz.Text, text.LOlz.Value);
                return result;
            }
            return result;
        });
        
        app.MapGet( "/Sasha", () => {
            
        });
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public string TestField { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class PostTest
{
    public int Age { get; set; }
    public string Name { get; set; }
}

public class LOlz
{
    public string Text { get; set; }
    public int Value { get; set; }
    public string LolzIteration(string Text, int Value)
    {
        var result = string.Empty;
        for (int i = 0; i < Value; i++)
            result += Text;
        return result;
    }
}

public class TestBody
{
    public LOlz LOlz { get; set; }
    public int Item { get; set; }
}