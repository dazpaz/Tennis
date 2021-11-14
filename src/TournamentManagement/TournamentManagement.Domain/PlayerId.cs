﻿using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public sealed class PlayerId : EntityId<PlayerId>
	{
		public PlayerId() : base() { }
		public PlayerId(Guid id) : base(id) { }
	}
}
