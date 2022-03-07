using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class AmendTournamentDto
	{
		public string Title { get; set; }
		public TournamentLevel TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid VenueId { get; set; }
	}
}
