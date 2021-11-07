using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class SetScoreTests
	{
		[Fact]
		public void CanCreateASetScore()
		{
			var set = new SetScore(6, 4);

			set.GamesOne.Should().Be(6);
			set.GamesTwo.Should().Be(4);
		}

		[Fact]
		public void SetScoreEquality()
		{
			var set1 = new SetScore(6, 4);
			var set2 = new SetScore(6, 4);
			var set3 = new SetScore(6, 5);
			var set4 = new SetScore(7, 5);

			set1.Equals(set2).Should().BeTrue();
			set1.Equals(set3).Should().BeFalse();
			set3.Equals(set4).Should().BeFalse();
		}

		[Theory]
		[InlineData(-1, 0)]
		[InlineData(0, -1)]
		[InlineData(100, 0)]
		[InlineData(0, 100)]
		public void CannotSetGamesValuesOutOfRange(int gamesOne, int gamesTwo)
		{
			Action act = () => new SetScore(gamesOne, gamesTwo);

			act.Should()
				.Throw<Exception>();
		}
	}
}
