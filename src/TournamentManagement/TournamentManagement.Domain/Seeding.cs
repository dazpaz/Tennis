using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class Seeding : ValueObject<Seeding>
	{
		public int Seed { get; }

		public Seeding(int seed)
		{
			Guard.ForIntegerOutOfRange(seed, 1, 32, nameof(seed));
			Seed = seed;
		}
	}
}
