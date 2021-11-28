using DomainDesign.Common;

namespace TournamentManagement.Domain.MatchAggregate
{
	public class SetScore : ValueObject<SetScore>
	{
		public int GamesOne { get; }
		public int GamesTwo { get; }

		public SetScore(int gamesOne, int gamesTwo)
		{
			Guard.AgainstIntegerOutOfRange(gamesOne, 0, 99, nameof(gamesOne));
			Guard.AgainstIntegerOutOfRange(gamesTwo, 0, 99, nameof(gamesTwo));

			GamesOne = gamesOne;
			GamesTwo = gamesTwo;
		}
	}
}
