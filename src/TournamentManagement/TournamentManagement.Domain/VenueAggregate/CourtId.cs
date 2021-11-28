using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.VenueAggregate
{
	public sealed class CourtId : EntityId<CourtId>
	{
		public CourtId() : base() { }
		public CourtId(Guid id) : base(id) { }
	}
}
