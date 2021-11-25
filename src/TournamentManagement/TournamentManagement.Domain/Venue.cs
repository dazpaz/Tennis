using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class Venue : Entity<VenueId>
	{
		public string Name { get; private set; }
		public Surface Surface { get; private set; }
		public ReadOnlyCollection<Court> Courts { get; }

		private readonly IList<Court> _courts;

		private Venue(VenueId id) : base(id)
		{
			_courts = new List<Court>();
			Courts = new ReadOnlyCollection<Court>(_courts);
		}

		public static Venue Create(string name, Surface surface)
		{
			Guard.AgainstNullOrEmptyString(name, nameof(name));

			var venue = new Venue(new VenueId())
			{
				Name = name,
				Surface = surface
			};

			return venue;
		}

		public void AddCourt(Court court)
		{
			GuardAgainstDuplicateCourtName(court.Name);
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
