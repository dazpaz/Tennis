using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class TournamentId : ValueObject<TournamentId>
	{
		public readonly Guid Id;

		public TournamentId()
		{
			Id = Guid.NewGuid();
		}

		public TournamentId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
