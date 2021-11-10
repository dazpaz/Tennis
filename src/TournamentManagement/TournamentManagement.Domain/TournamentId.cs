using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class TournamentId : EntityId
	{
		public TournamentId() : base() { }
		public TournamentId(Guid id) : base(id) { }
	}
}
