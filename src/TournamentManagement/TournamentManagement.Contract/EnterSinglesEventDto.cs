using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class EnterSinglesEventDto
	{
		public EventType EventType { get; set; }

		public Guid PlayerOneId { get; set; }
	}
}
