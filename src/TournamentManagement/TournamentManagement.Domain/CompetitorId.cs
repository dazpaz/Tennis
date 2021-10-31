using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class CompetitorId : ValueObject<CompetitorId>
	{
		public readonly Guid Id;

		public CompetitorId()
		{
			Id = Guid.NewGuid();
		}

		public CompetitorId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
