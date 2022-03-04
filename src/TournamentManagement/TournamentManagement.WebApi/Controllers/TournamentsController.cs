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
			var command = AddTournamentCommand.Create(tournamentDetails.Title,
				tournamentDetails.TournamentLevel, tournamentDetails.StartDate,
				tournamentDetails.EndDate, tournamentDetails.VenueId);

			if (command.IsFailure) return BadRequest(command.Error);

			Result<Guid> result = _dispatcher.Dispatch<Guid>(command.Value);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetTournament), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}

		[HttpPut("{id}")]
		public IActionResult AmendTournament(Guid id, [FromBody] AmendTournamentDto tournamentDetails)
		{
			var command = AmendTournamentCommand.Create(id, tournamentDetails.Title,
				tournamentDetails.TournamentLevel, tournamentDetails.StartDate,
				tournamentDetails.EndDate, tournamentDetails.VenueId);

			if (command.IsFailure) return BadRequest(command.Error);

			Result result = _dispatcher.Dispatch(command.Value);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpPost("{id}/Events")]
		public IActionResult AddEvent(Guid id, [FromBody] AddEventDto eventDetails)
		{
			var command = AddEventCommand.Create(id, eventDetails.EventType, eventDetails.EntrantsLimit,
				eventDetails.NumberOfSeeds, eventDetails.NumberOfSets, eventDetails.FinalSetType);

			if (command.IsFailure) return BadRequest(command.Error);

			Result result = _dispatcher.Dispatch(command.Value);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetEvent), new { id, eventType = eventDetails.EventType.ToString() }, null)
				: BadRequest(result.Error);
		}

		[HttpPut("{id}/Events/{eventType}")]
		public IActionResult AmendEvent(Guid id, string eventType, [FromBody] AmendEventDto eventDetails)
		{
			var command = AmendEventCommand.Create(id, eventType , eventDetails.EntrantsLimit,
				eventDetails.NumberOfSeeds, eventDetails.NumberOfSets, eventDetails.FinalSetType);

			if (command.IsFailure) return BadRequest(command.Error);

			Result result = _dispatcher.Dispatch(command.Value);

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
			var command = OpenForEntriesCommand.Create(id);

			if (command.IsFailure) return BadRequest(command.Error);

			Result result = _dispatcher.Dispatch(command.Value);

			return result.IsSuccess
				? Ok()
				: BadRequest(result.Error);
		}

		[HttpGet("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			var query = new GetTournamentDetails(id);

			Result<TournamentDetailsDto> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}

		[HttpGet("{id}/Events/{eventType}")]
		public IActionResult GetEvent(Guid id, string eventType)
		{
			if (!Enum.TryParse(eventType, out EventType type))
			{
				return BadRequest("Invalid Event Type");
			}

			var query = new GetEvent(id, type);
			Result<EventDto> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}

		[HttpGet]
		public IActionResult GetTournaments()
		{
			var query = new GetTournamentSummaryList();
			Result<List<TournamentSummaryDto>> result = _dispatcher.Dispatch(query);

			return result.IsSuccess 
				? Ok(result.Value) 
				: BadRequest(result.Error);
		}

		[HttpGet("Details")]
		public IActionResult GetTournamentDetails()
		{
			var query = new GetTournamentDetailsList();
			Result<List<TournamentDetailsDto>> result = _dispatcher.Dispatch(query);

			return result.IsSuccess
				? Ok(result.Value)
				: BadRequest(result.Error);
		}
	}
}
