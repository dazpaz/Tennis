using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class SetScore : ValueObject<SetScore>
	{
		public int GamesOne { get; }
		public int GamesTwo { get; }

		public SetScore(int gamesOne, int gamesTwo)
		{
			Guard.ForIntegerOutOfRange(gamesOne, 0, 99, "gamesOne");
			Guard.ForIntegerOutOfRange(gamesTwo, 0, 99, "gamesTwo");

			GamesOne = gamesOne;
			GamesTwo = gamesTwo;
		}
	}
}
