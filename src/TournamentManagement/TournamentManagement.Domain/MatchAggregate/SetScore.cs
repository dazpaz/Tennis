using Ardalis.GuardClauses;
using DomainDesign.Common;

namespace TournamentManagement.Domain.MatchAggregate
{
	public class SetScore : ValueObject<SetScore>
	{
		public int GamesOne { get; }
		public int GamesTwo { get; }

		public SetScore(int gamesOne, int gamesTwo)
		{
			Guard.Against.IntegerOutOfRange(gamesOne, 0, 99, nameof(gamesOne));
			Guard.Against.IntegerOutOfRange(gamesTwo, 0, 99, nameof(gamesTwo));

			GamesOne = gamesOne;
			GamesTwo = gamesTwo;
		}
	}
}
