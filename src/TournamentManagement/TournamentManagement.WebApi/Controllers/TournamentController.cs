using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using TournamentManagement.Application;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;
using TournamentManagement.WebApi.Dto;

namespace TournamentManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentController : ControllerBase
	{
		private readonly AddTournamentCommandHandler _addTournamentCommandHandler;

		public TournamentController(AddTournamentCommandHandler addTournamentCommandHandler)
		{
			_addTournamentCommandHandler = addTournamentCommandHandler;
		}

		[HttpPost]
		public IActionResult AddTournament([FromBody] AddTournamentDto tournament)
		{
			var command = MapDtoToAddTournamentCommand(tournament);

			Result<TournamentId> result = _addTournamentCommandHandler.Handle(command);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetTournament), new { id = result.Value.Id }, null)
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			return Ok($"Tournament {id} to ho here");
		}

		private static AddTournamentCommand MapDtoToAddTournamentCommand(AddTournamentDto tournament)
		{
			return new AddTournamentCommand()
			{
				Title = tournament.Title,
				TournamentLevel = tournament.TournamentLevel,
				StartDate = tournament.StartDate,
				EndDate = tournament.EndDate,
				VenueId = new VenueId(tournament.VenueId)
			};
		}
	}
}
