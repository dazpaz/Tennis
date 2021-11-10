using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class RoundId : EntityId
	{
		public RoundId() : base() { }
		public RoundId(Guid id) : base(id) {}
	}
}
