using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TournamentManagement.Application;
using TournamentManagement.Application.Commands;
using TournamentManagement.Contract;
using TournamentManagement.Query;

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

		#region Commands

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

		[HttpPut("{id}")]
		public IActionResult AmendTournament(Guid id, [FromBody] AmendTournamentDto tournamentDetails)
		{
			var command = AmendTournamentCommand.Create(id, tournamentDetails.Title,
				tournamentDetails.TournamentLevel, tournamentDetails.StartDate,
				tournamentDetails.EndDate, tournamentDetails.VenueId);

			return ExecuteCommand(command);
		}

		[HttpPut("{id}/Events/{eventType}")]
		public IActionResult AmendEvent(Guid id, string eventType, [FromBody] AmendEventDto eventDetails)
		{
			var command = AmendEventCommand.Create(id, eventType , eventDetails.EntrantsLimit,
				eventDetails.NumberOfSeeds, eventDetails.NumberOfSets, eventDetails.FinalSetType);

			return ExecuteCommand(command);
		}

		[HttpDelete("{id}/Events/{eventType}")]
		public IActionResult RemoveEvent(Guid id, string eventType)
		{
			var command = RemoveEventCommand.Create(id, eventType);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/OpenForEntries")]
		public IActionResult OpenForEntries(Guid id)
		{
			var command = OpenForEntriesCommand.Create(id);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/EnterSinglesEvent")]
		public IActionResult EnterSinglesEvent(Guid id, [FromBody] EnterSinglesEventDto entryDetails)
		{
			var command = EnterSinglesEventCommand.Create(id, entryDetails.EventType,
				entryDetails.PlayerOneId);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/EnterDoublesEvent")]
		public IActionResult EnterDoublesEvent(Guid id, [FromBody] EnterDoublesEventDto entryDetails)
		{
			var command = EnterDoublesEventCommand.Create(id, entryDetails.EventType,
				entryDetails.PlayerOneId, entryDetails.PlayerTwoId);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/WithdrawFromSinglesEvent")]
		public IActionResult WithdrawFromSinglesEvent(Guid id, [FromBody] WithdrawFromSinglesEventDto entryDetails)
		{
			var command = WithdrawFromSinglesEventCommand.Create(id, entryDetails.EventType,
				entryDetails.PlayerOneId);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/WithdrawFromDoublesEvent")]
		public IActionResult WithdrawFromDoublesEvent(Guid id, [FromBody] WithdrawFromDoublesEventDto entryDetails)
		{
			var command = WithdrawFromDoublesEventCommand.Create(id, entryDetails.EventType,
				entryDetails.PlayerOneId, entryDetails.PlayerTwoId);

			return ExecuteCommand(command);
		}

		[HttpPost("{id}/CloseEntries")]
		public IActionResult CloseEntries(Guid id)
		{
			var command = CloseEntriesCommand.Create(id);

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

		#endregion

		#region Queries

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
			var query = GetEventDetails.Create(id, eventType);

			if (query.IsFailure) return BadRequest(query.Error);

			Result<EventDto> result = _dispatcher.Dispatch(query.Value);

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

		#endregion
	}
}
