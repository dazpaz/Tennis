using Microsoft.AspNetCore.Mvc;

namespace Players.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayersController : ControllerBase
	{

		private readonly ILogger<PlayersController> _logger;

		public PlayersController(ILogger<PlayersController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<string> Get()
		{
			return new List<string>
			{
				"Medvedev",
				"Zverev",
				"Nadal",
				"Alxaraz",
				"Tsitsipas"
			};
		}
	}
}