using System;
using System.Collections.Generic;

namespace TournamentManagement.Contract
{
	public class TournamentDetailsDto : TournamentSummaryDto
	{
		public List<EventDto> Events { get; }

		public TournamentDetailsDto()
		{
			Events = new List<EventDto>();
		}
	}
}
