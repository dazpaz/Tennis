using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TournamentManagement.Application;
using TournamentManagement.Contract;

namespace TournamentManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentController : ControllerBase
	{
		private readonly CommandDispatcher _dispatcher;

		public TournamentController(CommandDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		[HttpPost]
		public IActionResult AddTournament([FromBody] AddTournamentDto tournamentDetails)
		{
			var command = new AddTournamentCommand(tournamentDetails.Title,
				tournamentDetails.TournamentLevel, tournamentDetails.StartDate,
				tournamentDetails.EndDate, tournamentDetails.VenueId);

			Result<Guid> result = _dispatcher.Dispatch<Guid>(command);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetTournament), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}

		[HttpPut("{id}")]
		public IActionResult AmendTournament(Guid id, [FromBody] AmendTournamentDto tournamentDetails)
		{
			var command = new AmendTournamentCommand(id, tournamentDetails.Title,
				tournamentDetails.TournamentLevel, tournamentDetails.StartDate,
				tournamentDetails.EndDate, tournamentDetails.VenueId);

			Result result = _dispatcher.Dispatch(command);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			return Ok($"Tournament {id} to go here");
		}


		[HttpGet]
		public IActionResult GetTournaments()
		{
			var query = new GetTournamentSummaryQuery();
			Result<List<TournamentSummaryDto>> result = _dispatcher.Dispatch(query);

			return result.IsSuccess 
				? Ok(result.Value) 
				: BadRequest(result.Error);
		}
	}
}
