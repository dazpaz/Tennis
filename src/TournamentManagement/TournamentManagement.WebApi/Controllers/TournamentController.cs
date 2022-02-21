using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using TournamentManagement.Application;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

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
			var command = new AddTournamentCommand()
			{
				Title = tournamentDetails.Title,
				TournamentLevel = tournamentDetails.TournamentLevel,
				StartDate = tournamentDetails.StartDate,
				EndDate = tournamentDetails.EndDate,
				VenueId = new VenueId(tournamentDetails.VenueId)
			};

			Result<TournamentId> result = _dispatcher.Dispatch<TournamentId>(command);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetTournament), new { id = result.Value.Id }, null)
				: BadRequest(result.Error);
		}

		[HttpPut("{id}")]
		public IActionResult AmendTournament(Guid id, [FromBody] AmendTournamentDto tournamentDetails)
		{
			var command = new AmendTournamentCommand()
			{
				Id = new TournamentId(id),
				Title = tournamentDetails.Title,
				TournamentLevel = tournamentDetails.TournamentLevel,
				StartDate = tournamentDetails.StartDate,
				EndDate = tournamentDetails.EndDate,
				VenueId = new VenueId(tournamentDetails.VenueId)
			};

			Result result = _dispatcher.Dispatch(command);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			return Ok($"Tournament {id} to ho here");
		}
	}
}
