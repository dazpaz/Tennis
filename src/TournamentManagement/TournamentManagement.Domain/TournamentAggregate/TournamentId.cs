using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public sealed class TournamentId : EntityId<TournamentId>
	{
		public TournamentId() : base() { }
		public TournamentId(Guid id) : base(id) { }
	}
}
