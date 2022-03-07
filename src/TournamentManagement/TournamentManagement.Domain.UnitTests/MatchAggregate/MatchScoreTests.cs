using FluentAssertions;
using System;
using System.Collections.Generic;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.MatchAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.MatchAggregate
{
	public class MatchScoreTests
	{
		[Fact]
		public void WhenCreatingMatchScoreWithNullSetScoresTheMatchScoreShouldBeZeroZeroWithUnknownWinner()
		{
			var format = new MatchFormat(3, SetType.TwoGamesClear);

			var matchScore = new MatchScore(format, null);

			matchScore.Sets[0].Should().Be(0);
			matchScore.Sets[1].Should().Be(0);
			matchScore.Winner.Should().Be(Winner.Unknown);
			matchScore.SetScores.Count.Should().Be(0);
		}

		[Fact]
		public void WhenCreatingMatchScoreWithZeroSetScoresTheMatchScoreShouldBeZeroZeroWithUnknownWinner()
		{
			var format = new MatchFormat(3, SetType.TwoGamesClear);

			var matchScore = new MatchScore(format, new List<SetScore>());

			matchScore.Sets[0].Should().Be(0);
			matchScore.Sets[1].Should().Be(0);
			matchScore.Winner.Should().Be(Winner.Unknown);
			matchScore.SetScores.Count.Should().Be(0);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 4)]
		[InlineData(5, 6)]
		public void CannotCreateMatchScoreWhenTheNumberOfSetScoresExceedsNumberOfSetsInTheMatch(
			int matchSets, int setScoresSupplied)
		{
			var format = new MatchFormat(matchSets, SetType.TwoGamesClear);
			var setScores = new List<SetScore>();
			for (int i = 0; i < setScoresSupplied; i++)
			{
				setScores.Add(new SetScore(0, 0));
			}

			Action act = () => new MatchScore(format, setScores);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Match score has {setScoresSupplied} sets, but can only have up to {matchSets} sets");
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(3, 2)]
		[InlineData(5, 3)]
		public void CompetitorOneCanBeDeterminedTheWinnerOfTheMatch(int matchSets, int setsPlayed)
		{
			var format = new MatchFormat(matchSets, SetType.TwoGamesClear);
			var setScores = new List<SetScore>();
			for (int i = 0; i < setsPlayed; i++)
			{
				setScores.Add(new SetScore(6, 4));
			}

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(setsPlayed);
			matchScore.Sets[1].Should().Be(0);
			matchScore.Winner.Should().Be(Winner.Competitor1);
			matchScore.SetScores.Count.Should().Be(setsPlayed);

			foreach (var setScore in matchScore.SetScores)
			{
				setScore.GamesOne.Should().Be(6);
				setScore.GamesTwo.Should().Be(4);
			}
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(3, 2)]
		[InlineData(5, 3)]
		public void CompetitorTwoCanBeDeterminedTheWinnerOfTheMatch(int matchSets, int setsPlayed)
		{
			var format = new MatchFormat(matchSets, SetType.TwoGamesClear);
			var setScores = new List<SetScore>();
			for (int i = 0; i < setsPlayed; i++)
			{
				setScores.Add(new SetScore(4, 6));
			}

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(0);
			matchScore.Sets[1].Should().Be(setsPlayed);
			matchScore.Winner.Should().Be(Winner.Competitor2);
			matchScore.SetScores.Count.Should().Be(setsPlayed);

			foreach (var setScore in matchScore.SetScores)
			{
				setScore.GamesOne.Should().Be(4);
				setScore.GamesTwo.Should().Be(6);
			}
		}

		[Fact]
		public void CompetitorOneCanWinAFiveSetMatchInFiveSets()
		{
			var format = new MatchFormat(5, SetType.TwoGamesClear);
			var setScores = new List<SetScore>
			{
				new SetScore(7, 6),
				new SetScore(6, 7),
				new SetScore(4, 6),
				new SetScore(6, 3),
				new SetScore(7, 5)
			};

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(3);
			matchScore.Sets[1].Should().Be(2);
			matchScore.Winner.Should().Be(Winner.Competitor1);
		}

		[Fact]
		public void CompetitorTwoCanWinAFiveSetMatchInFourSets()
		{
			var format = new MatchFormat(5, SetType.TwoGamesClear);
			var setScores = new List<SetScore>
			{
				new SetScore(7, 6),
				new SetScore(6, 7),
				new SetScore(4, 6),
				new SetScore(5, 7)
			};

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(1);
			matchScore.Sets[1].Should().Be(3);
			matchScore.Winner.Should().Be(Winner.Competitor2);
		}

		[Fact]
		public void IfNoPlayerWinsEnoughSetsInAFiveSetMatchTheWinnerShouldBeSetToUnknown()
		{
			var format = new MatchFormat(5, SetType.TwoGamesClear);
			var setScores = new List<SetScore>
			{
				new SetScore(7, 6),
				new SetScore(6, 7),
				new SetScore(7, 6),
				new SetScore(5, 7)
			};

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(2);
			matchScore.Sets[1].Should().Be(2);
			matchScore.Winner.Should().Be(Winner.Unknown);
		}

		[Fact]
		public void IfMatchIsStoppedInTheMiddleOfASetTheSetScoreIsCorrect()
		{
			var format = new MatchFormat(5, SetType.TwoGamesClear);
			var setScores = new List<SetScore>
			{
				new SetScore(7, 6),
				new SetScore(7, 6),
				new SetScore(3, 4)
			};

			var matchScore = new MatchScore(format, setScores);

			matchScore.Sets[0].Should().Be(2);
			matchScore.Sets[1].Should().Be(0);
			matchScore.Winner.Should().Be(Winner.Unknown);
		}
	}
}
