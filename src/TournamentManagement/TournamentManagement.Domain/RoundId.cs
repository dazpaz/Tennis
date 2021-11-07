using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class RoundId : ValueObject<RoundId>
	{
		public readonly Guid Id;

		public RoundId()
		{
			Id = Guid.NewGuid();
		}

		public RoundId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
