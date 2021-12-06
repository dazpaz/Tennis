using Microsoft.AspNetCore.Mvc;
using System;
using TournamentManagement.WebApi.Dto;

namespace TournamentManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentController : ControllerBase
	{
		[HttpGet]
		[Route("{id}")]
		public IActionResult GetTournament(Guid id)
		{
			// call to query handler to get the tournament
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
		public IActionResult Create([FromBody] CreateTournamentDto tournament)
		{
			// call to create tournament command to create the tournament
			// if fails return 400 - you were bad
			// if success return created with location (question - sould we rturn a body)

			return CreatedAtAction(nameof(GetTournament), new { id = Guid.NewGuid() }, tournament);
		}

		[HttpPut]
		[Route("{id}")]
		public IActionResult Update(Guid id, [FromBody] UpdateTournamentDto tournament)
		{
			// call to update tournament command to update the tournament
			// if fails return 400 - you were bad
			// if success return created with location and 

			return NoContent();
		}
	}
}
