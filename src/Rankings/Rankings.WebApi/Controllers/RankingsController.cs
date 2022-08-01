using Microsoft.AspNetCore.Mvc;

namespace Rankings.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RankingsController : ControllerBase
	{

		private readonly ILogger<RankingsController> _logger;

		public RankingsController(ILogger<RankingsController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetRankings")]
		public IEnumerable<string> Get()
		{
			return new List<string>
			{
				"1 - Medvedev",
				"2 - Zverev",
				"3 - Nadal",
				"4 - Alxaraz",
				"5 - Tsitsipas"
			};
		}
	}
}