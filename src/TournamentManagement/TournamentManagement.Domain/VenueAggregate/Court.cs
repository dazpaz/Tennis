﻿using Ardalis.GuardClauses;
using DomainDesign.Common;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TournamentManagement.Domain.UnitTests")]

namespace TournamentManagement.Domain.VenueAggregate
{
	public class Court : Entity<CourtId>
	{
		public const int MinCapacity = 0;
		public const int MaxCapacity = 25000;

		public string Name { get; private set; }
		public int Capacity { get; private set; }
		public VenueId VenueId { get; private set; }

		private Court(CourtId id) : base(id)
		{
		}

		internal static Court Create(CourtId id, string name, int capacity, VenueId venueId)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));
			Guard.Against.IntegerOutOfRange(capacity, MinCapacity, MaxCapacity, nameof(capacity));

			var court = new Court(id)
			{
				Name = name,
				Capacity = capacity,
				VenueId = venueId
			};

			return court;
		}

		public void UpdateCapacity(int newCapacity)
		{
			Capacity = Guard.Against.IntegerOutOfRange(newCapacity, MinCapacity, MaxCapacity, nameof(newCapacity));
		}

		public void RenameCourt(string newName)
		{
			Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName));
		}
	}
}
