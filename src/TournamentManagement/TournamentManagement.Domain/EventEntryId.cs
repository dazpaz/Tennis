using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class EventEntryId : ValueObject<EventEntryId>
	{
		public readonly Guid Id;

		public EventEntryId()
		{
			Id = Guid.NewGuid();
		}

		public EventEntryId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
