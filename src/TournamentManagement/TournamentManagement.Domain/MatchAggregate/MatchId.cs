using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.MatchAggregate
{
	public sealed class MatchId : EntityId<MatchId>
	{
		public MatchId() : base() { }
		public MatchId(Guid id) : base(id) { }
	}
}
