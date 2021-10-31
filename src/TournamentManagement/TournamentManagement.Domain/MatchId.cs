using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class MatchId : ValueObject<MatchId>
	{
		public readonly Guid Id;

		public MatchId()
		{
			Id = Guid.NewGuid();
		}

		public MatchId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
