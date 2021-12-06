using System;

namespace TournamentManagement.WebApi.Dto
{
	public class TournamentSummaryDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public int TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid VenueId { get; set; }
	}
}
