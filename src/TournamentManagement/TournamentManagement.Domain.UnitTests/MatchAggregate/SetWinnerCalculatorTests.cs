using FluentAssertions;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.MatchAggregate;
using Xunit;


namespace TournamentManagement.Domain.UnitTests.MatchAggregate
{
	public class SetWinnerCalculatorTests
	{
		[Theory]
		[InlineData(7, 6, Winner.Competitor1)]
		[InlineData(7, 5, Winner.Competitor1)]
		[InlineData(6, 4, Winner.Competitor1)]
		[InlineData(6, 0, Winner.Competitor1)]
		[InlineData(6, 7, Winner.Competitor2)]
		[InlineData(5, 7, Winner.Competitor2)]
		[InlineData(4, 6, Winner.Competitor2)]
		[InlineData(0, 6, Winner.Competitor2)]
		[InlineData(6, 6, Winner.Unknown)]
		[InlineData(6, 5, Winner.Unknown)]
		[InlineData(5, 6, Winner.Unknown)]
		[InlineData(5, 3, Winner.Unknown)]
		[InlineData(3, 5, Winner.Unknown)]
		public void CanCalculateTheWinnerOfASetWhenTheSetTypeIsTieBreak(int gamesOne, int gamesTwo, Winner expectedWinner)
		{
			var winner = SetWinnerCalculator.GetWinner(new SetScore(gamesOne, gamesTwo), SetType.TieBreak);

			winner.Should().Be(expectedWinner);
		}

		[Theory]
		[InlineData(13, 12, Winner.Competitor1)]
		[InlineData(13, 11, Winner.Competitor1)]
		[InlineData(7, 5, Winner.Competitor1)]
		[InlineData(6, 4, Winner.Competitor1)]
		[InlineData(6, 0, Winner.Competitor1)]
		[InlineData(12, 13, Winner.Competitor2)]
		[InlineData(11, 13, Winner.Competitor2)]
		[InlineData(5, 7, Winner.Competitor2)]
		[InlineData(4, 6, Winner.Competitor2)]
		[InlineData(0, 6, Winner.Competitor2)]
		[InlineData(12, 11, Winner.Unknown)]
		[InlineData(7, 6, Winner.Unknown)]
		[InlineData(11, 12, Winner.Unknown)]
		[InlineData(6, 7, Winner.Unknown)]
		[InlineData(6, 6, Winner.Unknown)]
		[InlineData(6, 5, Winner.Unknown)]
		[InlineData(5, 6, Winner.Unknown)]
		[InlineData(5, 3, Winner.Unknown)]
		[InlineData(3, 5, Winner.Unknown)]
		public void CanCalculateTheWinnerOfASetWhenTheSetTypeIsTieBreakAtTwelveAll(int gamesOne, int gamesTwo, Winner expectedWinner)
		{
			var winner = SetWinnerCalculator.GetWinner(new SetScore(gamesOne, gamesTwo), SetType.TieBreakAtTwelveAll);

			winner.Should().Be(expectedWinner);
		}

		[Theory]
		[InlineData(7, 5, Winner.Competitor1)]
		[InlineData(6, 4, Winner.Competitor1)]
		[InlineData(6, 0, Winner.Competitor1)]
		[InlineData(5, 7, Winner.Competitor2)]
		[InlineData(4, 6, Winner.Competitor2)]
		[InlineData(0, 6, Winner.Competitor2)]
		[InlineData(20, 19, Winner.Unknown)]
		[InlineData(19, 20, Winner.Unknown)]
		[InlineData(7, 6, Winner.Unknown)]
		[InlineData(6, 7, Winner.Unknown)]
		[InlineData(6, 6, Winner.Unknown)]
		[InlineData(6, 5, Winner.Unknown)]
		[InlineData(5, 6, Winner.Unknown)]
		[InlineData(5, 3, Winner.Unknown)]
		[InlineData(3, 5, Winner.Unknown)]
		public void CanCalculateTheWinnerOfASetWhenTheSetTypeIsTwoGamesClear(int gamesOne, int gamesTwo, Winner expectedWinner)
		{
			var winner = SetWinnerCalculator.GetWinner(new SetScore(gamesOne, gamesTwo), SetType.TwoGamesClear);

			winner.Should().Be(expectedWinner);
		}

		[Theory]
		[InlineData(11, 9, Winner.Competitor1)]
		[InlineData(10, 8, Winner.Competitor1)]
		[InlineData(10, 4, Winner.Competitor1)]
		[InlineData(10, 0, Winner.Competitor1)]
		[InlineData(9, 11, Winner.Competitor2)]
		[InlineData(8, 10, Winner.Competitor2)]
		[InlineData(4, 10, Winner.Competitor2)]
		[InlineData(0, 10, Winner.Competitor2)]
		[InlineData(10, 9, Winner.Unknown)]
		[InlineData(9, 10, Winner.Unknown)]
		[InlineData(10, 10, Winner.Unknown)]
		[InlineData(9, 7, Winner.Unknown)]
		[InlineData(7, 9, Winner.Unknown)]
		public void CanCalculateTheWinnerOfASetWhenTheSetTypeIsChampionsTieBreak(int gamesOne, int gamesTwo, Winner expectedWinner)
		{
			var winner = SetWinnerCalculator.GetWinner(new SetScore(gamesOne, gamesTwo), SetType.ChampionsTieBreak);

			winner.Should().Be(expectedWinner);
		}

	}
}
