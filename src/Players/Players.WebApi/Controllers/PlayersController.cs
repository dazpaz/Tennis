using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Microsoft.AspNetCore.Mvc;
using Players.Contract;
using Players.WebApi.Factory;

namespace Players.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
	private readonly IMessageDispatcher _dispatcher;
	private readonly ICommandFactory _commandFactory;
	private readonly IQueryFactory _queryFactory;

	public PlayersController(IMessageDispatcher dispatcher,
		ICommandFactory commandFactory,
		IQueryFactory queryFactory)
	{
		_dispatcher = dispatcher;
		_commandFactory = commandFactory;
		_queryFactory = queryFactory;
	}

	[HttpPost]
	public IActionResult RegisterPlayer([FromBody] RegisterPlayerDto playerDetails)
	{
		var command = _commandFactory.CreateRegisterPlayerCommand(playerDetails);
		if (command.IsFailure) return BadRequest(command.Error);

		Result<Guid> result = _dispatcher.Dispatch<Guid>(command.Value);

		return result.IsSuccess
			? CreatedAtAction(nameof(GetPlayer), new { id = result.Value }, null)
			: BadRequest(result.Error);
	}

	[HttpPost("{id}/UpdateSinglesRanking")]
	public IActionResult UpdateSinglesRanking(Guid id, [FromBody] UpdateRankingDto newRanking)
	{
		var command = _commandFactory.CreateUpdateSinglesRankingCommand(id, newRanking);
		return ExecuteCommand(command);
	}

	[HttpPost("{id}/UpdateDoublesRanking")]
	public IActionResult UpdateDoublesRanking(Guid id, [FromBody] UpdateRankingDto newRanking)
	{
		var command = _commandFactory.CreateUpdateDoublesRankingCommand(id, newRanking);
		return ExecuteCommand(command);
	}

	private IActionResult ExecuteCommand(Result<ICommand> command)
	{
		if (command.IsFailure) return BadRequest(command.Error);

		Result result = _dispatcher.Dispatch(command.Value);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpGet("{id}")]
	public IActionResult GetPlayer(Guid id)
	{
		var query = _queryFactory.CreateGetPlayerDetailsQuery(id);

		Result<PlayerDetailsDto> result = _dispatcher.Dispatch(query);

		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}

	[HttpGet]
	public IActionResult GetPlayers()
	{
		var query = _queryFactory.CreateGetPlayerSummaryListQuery();

		Result<IList<PlayerSummaryDto>> result = _dispatcher.Dispatch(query);

		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
}