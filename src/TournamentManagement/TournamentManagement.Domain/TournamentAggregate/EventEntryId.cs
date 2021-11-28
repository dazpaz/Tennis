using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public sealed class EventEntryId : EntityId<EventEntryId>
	{
		public EventEntryId() : base() { }
		public EventEntryId(Guid id) : base(id) { }
	}
}
