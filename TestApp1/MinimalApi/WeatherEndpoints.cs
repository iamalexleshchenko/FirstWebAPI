namespace TestApp1.MinimalApi;
using TestApp1.DTOs;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
            "Balmy", "Hot", "Sweltering", "Scorching"
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
                    }).ToArray();

                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        app.MapPost("/huinya", (PostTest test) =>
        {
            return $"Idi na hui, {test.Name}"; // ❗Заменить
        });

        app.MapPost("/lolz", (TestBody text) =>
        {
            string result = "";
            for (; text.Item > 0; text.Item--)
            {
                result += text.LOlz.LolzIteration(text.LOlz.Text, text.LOlz.Value);
                return result; // ❗Ошибка — return в цикле. Вынести после
            }
            return result;
        });
    }
}