﻿using DomainDesign.Common;
using System;


namespace TournamentManagement.Domain.VenueAggregate
{
	public class VenueId : EntityId<VenueId>
	{
		public VenueId() : base() { }
		public VenueId(Guid id) : base(id) { }
	}
}
