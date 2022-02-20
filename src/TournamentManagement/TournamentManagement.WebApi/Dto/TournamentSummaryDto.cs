﻿using System;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.WebApi.Dto
{
	public class TournamentSummaryDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public TournamentLevel TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid VenueId { get; set; }
		public string VenueTitle { get; set; }
	}
}
