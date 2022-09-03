using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Players.Contract;
using Players.WebApi.Factory;

namespace Players.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CountriesController : ControllerBase
	{
		private readonly IMessageDispatcher _dispatcher;
		private readonly ICommandFactory _commandFactory;
		private readonly IQueryFactory _queryFactory;

		public CountriesController(IMessageDispatcher dispatcher,
			ICommandFactory commandFactory,
			IQueryFactory queryFactory)
		{
			_dispatcher = dispatcher;
			_commandFactory = commandFactory;
			_queryFactory = queryFactory;
		}

		[HttpPost]
		public IActionResult CreateCountry([FromBody] CreateCountryDto countryDetails)
		{
			var command = _commandFactory.CreateCreateCountryCommand(countryDetails);
			if (command.IsFailure) return BadRequest(command.Error);

			Result<Guid> result = _dispatcher.Dispatch<Guid>(command.Value);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetCountry), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetCountry(Guid id)
		{
			var query = _queryFactory.CreateGetCountryDetailsQuery(id);
			Result<CountryDetailsDto> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}

		[HttpGet]
		public IActionResult GetCountries()
		{
			var query = _queryFactory.CreateGetCountryDetailsListQuery();
			Result<IList<CountryDetailsDto>> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}
	}
}
