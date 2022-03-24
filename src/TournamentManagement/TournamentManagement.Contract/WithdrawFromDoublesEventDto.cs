using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public sealed class WithdrawFromDoublesEventDto
	{
		public EventType EventType { get; set; }

		public Guid PlayerOneId { get; set; }

		public Guid PlayerTwoId { get; set; }
	}
}
