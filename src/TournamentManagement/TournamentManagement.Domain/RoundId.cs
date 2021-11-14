using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public sealed class RoundId : EntityId<RoundId>
	{
		public RoundId() : base() { }
		public RoundId(Guid id) : base(id) {}
	}
}
