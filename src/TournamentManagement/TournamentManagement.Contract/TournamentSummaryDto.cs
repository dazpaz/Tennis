using System;

namespace TournamentManagement.Contract
{
	public class TournamentSummaryDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string State { get; set; }
		public string VenueName { get; set; }
		public int NumberOfEvents { get; set; }
	}
}
