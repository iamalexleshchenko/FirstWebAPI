using Microsoft.EntityFrameworkCore;
using TestApp1.Database;
using TestApp1.Services;
using TestApp1.MinimalApi;

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
        //AddMinimalApi(app);

        // асинхронный запуск объекта веб-приложения
        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DatabaseContext>((options) =>
        {
            options.UseNpgsql("Host=localhost;Port=5432;Database=FirstWebAPI;Username=postgres;Password=fc207hbl");
            //options.UseSqlite("Data Source=test.db");
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
        app.MapWeatherEndpoints();
    }
}