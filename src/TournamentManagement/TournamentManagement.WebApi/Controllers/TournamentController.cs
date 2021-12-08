using Microsoft.AspNetCore.Mvc;
using System;
using TournamentManagement.WebApi.Dto;

namespace TournamentManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentController : ControllerBase
	{
		[HttpGet()]
		public IActionResult GetTournaments()
		{
			// call to query tournament handler to get the tournaments
			// convert to list of dtos
			// return ok with the list

			return Ok("List of tournaments goes here");
		}

		[HttpGet("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			// call to query tournament handler to get the tournament by id
			// if not found - return 404
			// if found convert to dto and return the summary details

			return Ok(new TournamentSummaryDto
			{
				Id = id,
				Title = "Wimbledon",
				TournamentLevel = 2000,
				StartDate = DateTime.Today,
				EndDate = DateTime.Today.AddDays(14),
				VenueId = Guid.NewGuid()
			});
		}

		[HttpPost]
		public IActionResult AddTournament([FromBody] AddTournamentDto tournament)
		{
			// if not valid request return bad request
			// call to create tournament command to create the tournament
			// if fails return 400 - you were bad
			// if success return created with location (question - sould we rturn a body)

			return CreatedAtAction(nameof(GetTournament), new { id = Guid.NewGuid() }, null);
		}

		[HttpPut("{id}")]
		public IActionResult AmmendTournament(Guid id, [FromBody] AmendTournamentDto tournament)
		{
			// call to update tournament command to update the tournament
			// if tournament does not exist - 404
			// if fails return 400 - you were bad
			// if success return no content

			return NoContent();
		}

		[HttpGet("{id}/events")]
		public IActionResult GetEvents(Guid id)
		{
			// call query to get all events for this tournament

			return Ok("List of events goes here");
		}


		[HttpGet("{id}/events/{eventType}")]
		public IActionResult GetEvent(Guid id, string eventType)
		{
			// call query
			// if not found - 404
			// iff error 400
			// otherwse return 200 and the event

			return Ok("Event goes here");
		}

		[HttpPost("{id}/addevent")]
		public IActionResult AddEvent(Guid id, [FromBody] CreateEventDto tennisEvent)
		{
			// call command to add the event
			// if tournament does not exist - return 404
			// if fails return a 400 - you were bad
			// if success return created

			return CreatedAtAction(nameof(GetEvent), new { id, tennisEvent.EventType }, null);
		}

		[HttpDelete("{id}/removeevent/{eventType}")]
		public IActionResult RemoveEvent(Guid id, string eventType)
		{
			// call command to add the event
			// if tournament does not exist - return 404
			// if fails return a 400 - you were bad
			// if success return created

			return Ok();
		}

		[HttpPost("{id}/openforentries")]
		public IActionResult OpenForEntries(Guid id)
		{
			// call into command to do the open command
			// if id does not extist - 404
			// any other error 400

			return NoContent();
		}
	}
}
