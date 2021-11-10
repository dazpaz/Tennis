using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class EventId : EntityId
	{
		public EventId() : base() { }
		public EventId(Guid id) : base(id) { }
	}
}
