using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TournamentManagement.Domain.VenueAggregate
{
	public class Venue : Entity<VenueId>, IAggregateRoot
	{
		public string Name { get; private set; }
		public Surface Surface { get; private set; }

		private readonly List<Court> _courts = new();
		public virtual IReadOnlyList<Court> Courts => _courts.ToList();

		protected Venue()
		{
		}

		private Venue(VenueId id) : base(id)
		{
		}

		public static Venue Create(VenueId id, string name, Surface surface)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));

			var venue = new Venue(id)
			{
				Name = name,
				Surface = surface
			};

			return venue;
		}

		public void AddCourt(CourtId id, string name, int capacity)
		{
			GuardAgainstDuplicateCourtName(name);
			var court = Court.Create(id, name, capacity);
			_courts.Add(court);
		}

		public void RemoveCourt(CourtId id)
		{
			var court = _courts.FirstOrDefault(c => c.Id == id);
			if (court != null)
			{
				_courts.Remove(court);
			}
		}

		private void GuardAgainstDuplicateCourtName(string name)
		{
			if (_courts.Any(c => c.Name == name))
			{
				throw new Exception($"Cannot add court to venue, court name already exists: {name}");
			}
		}
	}
}
