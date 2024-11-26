using GrpcServerAPI;
using Microsoft.AspNetCore.Mvc;

namespace GrpcClientAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ServerService.ServerServiceClient _grpcClient;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ServerService.ServerServiceClient grpcClient)
        {
            _logger = logger;
            _grpcClient = grpcClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("GetServerValue/{id}")]
        public async Task<string> GetServerValue(int id)
        {
            var response = await _grpcClient.GetByIdAsync(new GetByIdRequest { Id = id });
            return response.Value;
        }
    }
}
