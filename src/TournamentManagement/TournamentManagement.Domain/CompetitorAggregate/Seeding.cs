using Ardalis.GuardClauses;
using DomainDesign.Common;

namespace TournamentManagement.Domain.CompetitorAggregate
{
	public class Seeding : ValueObject<Seeding>
	{
		public int Seed { get; }

		protected Seeding()
		{
		}

		public Seeding(int seed)
		{
			Seed = Guard.Against.IntegerOutOfRange(seed, 1, 32, nameof(seed));
		}
	}
}
