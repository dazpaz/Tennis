using FluentAssertions;
using System;
using TournamentManagement.Domain.Common;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.Common
{
	public class MatchFormatTests
	{
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(5)]
		[Theory]
		public void CanCreateMatchFormatWithValidNumberOfSets(int numberOfSets)
		{
			MatchFormat format = new(numberOfSets, SetType.TwoGamesClear);
			format.NumberOfSets.Should().Be(numberOfSets);
		}

		[Fact]
		public void CanCreateOneSetMatchWithFinalSetTieBreak()
		{
			MatchFormat format = MatchFormat.OneSetMatchWithFinalSetTieBreak;
			format.NumberOfSets.Should().Be(1);
			format.FinalSetType.Should().Be(SetType.TieBreak);
		}

		[Fact]
		public void CanCreateThreeSetMatchWithFinalSetTieBreak()
		{
			MatchFormat format = MatchFormat.ThreeSetMatchWithFinalSetTieBreak;
			format.NumberOfSets.Should().Be(3);
			format.FinalSetType.Should().Be(SetType.TieBreak);
		}

		[Fact]
		public void CanCreateFiveSetMatchWithFinalSetTieBreak()
		{
			MatchFormat format = MatchFormat.FiveSetMatchWithFinalSetTieBreak;
			format.NumberOfSets.Should().Be(5);
			format.FinalSetType.Should().Be(SetType.TieBreak);
		}

		[Fact]
		public void CanCreateOneSetMatchWithTwoGamesClear()
		{
			MatchFormat format = MatchFormat.OneSetMatchWithTwoGamesClear;
			format.NumberOfSets.Should().Be(1);
			format.FinalSetType.Should().Be(SetType.TwoGamesClear);
		}

		[Fact]
		public void CanCreateThreeSetMatchWithTwoGamesClear()
		{
			MatchFormat format = MatchFormat.ThreeSetMatchWithTwoGamesClear;
			format.NumberOfSets.Should().Be(3);
			format.FinalSetType.Should().Be(SetType.TwoGamesClear);
		}

		[Fact]
		public void CanCreateFiveSetMatchWithTwoGamesClear()
		{
			MatchFormat format = MatchFormat.FiveSetMatchWithTwoGamesClear;
			format.NumberOfSets.Should().Be(5);
			format.FinalSetType.Should().Be(SetType.TwoGamesClear);
		}

		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(6)]
		[Theory]
		public void EnsureInvalidNumberOfSetsThrowsException(int numberOfSets)
		{
			Action act = () => new MatchFormat(numberOfSets, SetType.TieBreak);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"Invalid number of sets, {numberOfSets}.");
		}
	}
}
