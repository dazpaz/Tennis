using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class EventId : ValueObject<EventId>
	{
		public readonly Guid Id;

		public EventId()
		{
			Id = Guid.NewGuid();
		}

		public EventId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
