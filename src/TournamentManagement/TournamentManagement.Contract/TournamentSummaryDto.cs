using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class TournamentSummaryDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public TournamentLevel Level { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public TournamentState State { get; set; }
		public string VenueName { get; set; }
		public int NumberOfEvents { get; set; }
	}
}
