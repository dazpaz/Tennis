using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public sealed class EventId : EntityId<EventId>
	{
		public EventId() : base() { }
		public EventId(Guid id) : base(id) { }
	}
}
