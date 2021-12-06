using System;

namespace TournamentManagement.WebApi.Dto
{
	public class UpdateTournamentDto
	{
		public string Title { get; set; }
		public int TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid VenueId { get; set; }
	}
}
