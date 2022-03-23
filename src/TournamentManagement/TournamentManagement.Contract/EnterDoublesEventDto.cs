using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class EnterDoublesEventDto
	{
		public EventType EventType { get; set; }

		public Guid PlayerOneId { get; set; }

		public Guid PlayerTwoId { get; set; }
	}
}
