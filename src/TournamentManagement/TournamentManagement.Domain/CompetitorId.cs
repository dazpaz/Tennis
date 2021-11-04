using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class CompetitorId : ValueObject<CompetitorId>
	{
		public static CompetitorId Bye => new(new Guid("712917f4-cad8-48c7-a93a-3e8501e7e70e"));

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
