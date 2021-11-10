using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class EventEntryId : EntityId
	{
		public EventEntryId() : base() { }
		public EventEntryId(Guid id) : base(id) { }
	}
}
