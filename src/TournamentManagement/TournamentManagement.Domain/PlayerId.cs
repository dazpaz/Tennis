using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class PlayerId : EntityId
	{
		public PlayerId() : base() { }
		public PlayerId(Guid id) : base(id) { }
	}
}
