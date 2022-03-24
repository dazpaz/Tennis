using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public sealed class WithdrawFromSinglesEventDto
	{
		public EventType EventType { get; set; }

		public Guid PlayerOneId { get; set; }
	}
}
