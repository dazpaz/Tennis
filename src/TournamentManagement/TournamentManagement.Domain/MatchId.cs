using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class MatchId : EntityId
	{
		public MatchId() : base() { }
		public MatchId(Guid id) : base(id) { }
	}
}
