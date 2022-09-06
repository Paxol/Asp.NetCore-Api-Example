using DAMWeb.Shared.Models;
using DAMWeb.Shared.Requests;
using DAMWeb.Shared;
using MediatR;

namespace DAMWeb.Server.Handlers
{
    public class ForecastForDaysHandler : IRequestHandler<ForecastForDaysRequest, ApiResponse>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<ApiResponse> Handle(ForecastForDaysRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);

            return ApiResponse.Ok(Enumerable.Range(1, request.Days).Select(idx => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(idx),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
