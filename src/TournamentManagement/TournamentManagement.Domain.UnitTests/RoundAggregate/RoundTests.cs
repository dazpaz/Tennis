using FluentAssertions;
using System;
using TournamentManagement.Common;
using TournamentManagement.Domain.RoundAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.RoundAggregate
{
	public class RoundTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateRoundAndItIsCreatedCorrectly()
		{
			var tournament = CreateTestTournament();

			var round = Round.Create(tournament, EventType.MensSingles, 1, 128);

			round.Id.Id.Should().NotBe(Guid.Empty);
			round.Tournament.Should().Be(tournament);
			round.EventType.Should().Be(EventType.MensSingles);
			round.RoundNumber.Should().Be(1);
			round.CompetitorCount.Should().Be(128);
			round.Title.Should().Be("Round of 128");
		}

		[Fact]
		public void CannotCreateARoundWithNullTournament()
		{
			Action act = () => Round.Create(null, EventType.MensSingles, 1, 128);

			act.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'tournament')");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(8)]
		public void CannotCreateRoundWithRoundNumberOutOfRange(int roundNumber)
		{
			Action act = () => Round.Create(CreateTestTournament(), EventType.MensSingles, roundNumber, 128);

			act.Should()
				.Throw<ArgumentOutOfRangeException>()
				.WithMessage($"Value {roundNumber} must be between 1 and 7 (Parameter 'roundNumber')");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		public void CannotCreateRoundWithInvalidNumberOfCompetitors(int competitorCount)
		{
			Action act = () => Round.Create(CreateTestTournament(), EventType.MensSingles, 1, competitorCount);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"{competitorCount} is not one of the allowed values (Parameter 'competitorCount')");
		}

		[Theory]
		[InlineData(128, "Round of 128")]
		[InlineData(64, "Round of 64")]
		[InlineData(32, "Round of 32")]
		[InlineData(16, "Round of 16")]
		[InlineData(8, "Quarter-Final")]
		[InlineData(4, "Semi-Final")]
		[InlineData(2, "Final")]
		public void TitleOfTheRoundIsCalculatedFromTheNumberOfCompetitors(int competitorCount, string expectedTitle)
		{
			var round = Round.Create(CreateTestTournament(), EventType.MensSingles, 1, competitorCount);

			round.Title.Should().Be(expectedTitle);
		}

		private static Tournament CreateTestTournament()
		{
			var venue = Venue.Create(new VenueId(), "AETLA", Surface.Grass);
			var dates = new TournamentDates(DateTime.Today, DateTime.Today);
			return Tournament.Create((TournamentTitle)"Wimbledon", TournamentLevel.Masters500, dates, venue);
		}
	}
}
