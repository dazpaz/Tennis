using DomainDesign.Common;

namespace TournamentManagement.Domain.CompetitorAggregate
{
	public class Seeding : ValueObject<Seeding>
	{
		public int Seed { get; }

		public Seeding(int seed)
		{
			Guard.AgainstIntegerOutOfRange(seed, 1, 32, nameof(seed));
			Seed = seed;
		}
	}
}
