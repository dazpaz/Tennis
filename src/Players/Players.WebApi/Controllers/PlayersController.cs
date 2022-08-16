using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Players.Application.Commands;
using Players.Contract;

namespace Players.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayersController : ControllerBase
	{

		private readonly ILogger<PlayersController> _logger;
		private readonly IMessageDispatcher _dispatcher;

		public PlayersController(ILogger<PlayersController> logger, IMessageDispatcher dispatcher)
		{
			_logger = logger;
			_dispatcher = dispatcher;
		}

		[HttpPost]
		public IActionResult RegisterPlayer([FromBody] RegisterPlayerDto playerDetails)
		{
			var command = RegisterPlayerCommand.Create(playerDetails.FirstName, playerDetails.LastName, playerDetails.Gender,
				playerDetails.DateOfBirth, playerDetails.Plays, playerDetails.Height, playerDetails.Country);

			if (command.IsFailure) return BadRequest(command.Error);

			Result<Guid> result = _dispatcher.Dispatch<Guid>(command.Value);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetPlayer), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetPlayer(Guid id)
		{
			return Ok(id);

			//var query = new GetTournamentDetails(id);

			//Result<TournamentDetailsDto> result = _dispatcher.Dispatch(query);

			//return result.IsSuccess
			//	? Ok(result.Value)
			//	: BadRequest(result.Error);
		}
	}
}