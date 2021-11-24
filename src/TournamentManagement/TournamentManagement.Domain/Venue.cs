using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class Venue : Entity<VenueId>
	{
		public string Name { get; private set; }
		public Surface Surface { get; private set; }

		private Venue(VenueId id) : base(id)
		{
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
	}
}
