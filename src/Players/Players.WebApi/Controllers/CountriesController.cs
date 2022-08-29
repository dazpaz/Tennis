using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Players.Application.Commands;
using Players.Contract;
using Players.Query;

namespace Players.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CountriesController : ControllerBase
	{

		private readonly ILogger<PlayersController> _logger;
		private readonly IMessageDispatcher _dispatcher;

		public CountriesController(ILogger<PlayersController> logger, IMessageDispatcher dispatcher)
		{
			_logger = logger;
			_dispatcher = dispatcher;
		}

		[HttpPost]
		public IActionResult CreateCountry([FromBody] CreateCountryDto countryDetails)
		{
			var command = CreateCountryCommand.Create(countryDetails.ShortName, countryDetails.FullName);

			if (command.IsFailure) return BadRequest(command.Error);

			Result<Guid> result = _dispatcher.Dispatch<Guid>(command.Value);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetCountry), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetCountry(Guid id)
		{
			var query = new GetCountryDetails(id);

			Result<CountryDetailsDto> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}

		[HttpGet]
		public IActionResult GetCountries()
		{
			var query = new GetCountryDetailsList();
			Result<IList<CountryDetailsDto>> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}
	}
}
