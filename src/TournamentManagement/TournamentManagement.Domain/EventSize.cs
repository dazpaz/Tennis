using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class EventSize : ValueObject<EventSize>
	{
		private const int MaxNumberOfEntrants = 128;
		private static readonly int[] AllowedNumberOfSeeds = { 2, 4, 8, 16, 32 };
		private static readonly int[] EntrantsPerRound = { 2, 4, 8, 16, 32, 64, 128 };
		private static readonly int[] MinAllowedEntrants = { 2, 2, 4, 8, 16, 32, 64 };

		public int EntrantsLimit { get; }
		public int NumberOfSeeds { get; }
		public int NumberOfRounds { get; }
		public int MinimumEntrants { get; }

		public EventSize(int entrantsLimit, int numberOfSeeds)
		{
			Guard.AgainstValueNotInSetOfAllowedValues(numberOfSeeds, AllowedNumberOfSeeds, nameof(numberOfSeeds));
			Guard.AgainstIntegerOutOfRange(entrantsLimit, numberOfSeeds, MaxNumberOfEntrants, nameof(entrantsLimit));

			EntrantsLimit = entrantsLimit;
			NumberOfSeeds = numberOfSeeds;
			NumberOfRounds = CalculateNumberOfRounds(entrantsLimit);
			MinimumEntrants = MinAllowedEntrants[NumberOfRounds - 1]; 
		}

		private static int CalculateNumberOfRounds(int entrantsLimit)
		{
			int rounds = 0;
			for (int i = 0; i < EntrantsPerRound.Length; i++)
			{
				if (EntrantsPerRound[i] >= entrantsLimit)
				{
					rounds = i+1;
					break;
				}
			}

			return rounds;
		}
	}
}
