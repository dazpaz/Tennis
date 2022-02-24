using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TournamentManagement.Application;
using TournamentManagement.Application.Commands;
using TournamentManagement.Application.Queries;
using TournamentManagement.Contract;

namespace TournamentManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentsController : ControllerBase
	{
		private readonly MessageDispatcher _dispatcher;

		public TournamentsController(MessageDispatcher dispatcher)
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

		[HttpPost("{id}/Events")]
		public IActionResult AddEvent(Guid id, [FromBody] AddEventDto eventDetails)
		{
			var command = new AddEventCommand(id, eventDetails.EventType, eventDetails.EntrantsLimit,
				eventDetails.NumberOfSeeds, eventDetails.NumberOfSets, eventDetails.FinalSetType);

			Result result = _dispatcher.Dispatch(command);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetEvent), new { id, eventType = eventDetails.EventType.ToString() }, null)
				: BadRequest(result.Error);
		}

		[HttpPut("{id}/Events/{eventType}")]
		public IActionResult AmendEvent(Guid id, string eventType, [FromBody] AmendEventDto eventDetails)
		{
			if (!Enum.TryParse(eventType, out EventType type))
			{
				return BadRequest("Invalid Event Type");
			}

			var command = new AmendEventCommand(id, type, eventDetails.EntrantsLimit,
				eventDetails.NumberOfSeeds, eventDetails.NumberOfSets, eventDetails.FinalSetType);

			Result result = _dispatcher.Dispatch(command);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpDelete("{id}/Events/{eventType}")]
		public IActionResult RemoveEvent(Guid id, string eventType)
		{
			if (!Enum.TryParse(eventType, out EventType type))
			{
				return BadRequest("Invalid Event Type");
			}

			var command = new RemoveEventCommand(id, type);

			Result result = _dispatcher.Dispatch(command);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpPost("{id}/OpenForEntries")]
		public IActionResult OpenForEntries(Guid id)
		{
			var command = new OpenForEntriesCommand(id);

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

		[HttpGet("{id}/Events/{eventType}")]
		public IActionResult GetEvent(Guid id, string eventType)
		{
			return Ok($"Event {id} of type {eventType} to go here");
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
