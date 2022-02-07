using DomainDesign.Common;
using System.Collections.Generic;

namespace TournamentManagement.Domain.TournamentAggregate.Events
{
	public sealed class TournamentEntryOpened : IDomainEvent
	{
		public TournamentId TournamentId { get; }
		public IEnumerable<EventType> EventTypes { get; }

		public TournamentEntryOpened(TournamentId tournamentId, IEnumerable<EventType> eventTypes)
		{
			TournamentId = tournamentId;
			EventTypes = eventTypes;
		}
	}
}
