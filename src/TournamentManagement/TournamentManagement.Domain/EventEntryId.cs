using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public sealed class EventEntryId : EntityId<EventEntryId>
	{
		public EventEntryId() : base() { }
		public EventEntryId(Guid id) : base(id) { }
	}
}
