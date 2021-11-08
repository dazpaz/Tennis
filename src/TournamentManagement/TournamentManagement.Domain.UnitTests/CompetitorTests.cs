using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class CompetitorTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForASinglesEventAndItIsCreatedCorrectly()
		{
			var tournamentId = new TournamentId();
			var eventEntryId = Guid.NewGuid();
			var competitor = Competitor.Create(tournamentId, EventType.MensSingles, eventEntryId, 1,
				new List<string>() { "Steve Serve" });

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.TournamentId.Should().Be(tournamentId);
			competitor.EventType.Should().Be(EventType.MensSingles);
			competitor.Seeding.Should().Be(1);
			competitor.PlayerNames.Count.Should().Be(1);
			competitor.PlayerNames[0].Should().Be("Steve Serve");
		}

		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForADoublesEventAndItIsCreatedCorrectly()
		{
			var tournamentId = new TournamentId();
			var eventEntryId = Guid.NewGuid();
			var competitor = Competitor.Create(tournamentId, EventType.MensDoubles, eventEntryId, 1,
				new List<string>() { "Steve Serve", "Vernon Volley" });

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.TournamentId.Should().Be(tournamentId);
			competitor.EventType.Should().Be(EventType.MensDoubles);
			competitor.Seeding.Should().Be(1);
			competitor.PlayerNames.Count.Should().Be(2);
			competitor.PlayerNames[0].Should().Be("Steve Serve");
			competitor.PlayerNames[1].Should().Be("Vernon Volley");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(33)]
		public void CannotCreateCompetitorWithInvalidSeeding(int seeding)
		{
			var tournamentId = new TournamentId();
			var eventEntryId = Guid.NewGuid();
			
			Action act = () => Competitor.Create(tournamentId, EventType.MensSingles, eventEntryId, seeding,
				new List<string>() { "Steve Serve" });

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Value {seeding} must be between 0 and 32 (Parameter 'seeding')");
		}

		[Theory]
		[InlineData(EventType.MensSingles, 2)]
		[InlineData(EventType.WomensSingles, 2)]
		[InlineData(EventType.MensDoubles, 1)]
		[InlineData(EventType.WomensDoubles, 1)]
		[InlineData(EventType.MixedDoubles, 1)]
		public void CannotCreateCompetitorWithWrongNumberOfPlayers(EventType eventType, int numberOfPlayers)
		{
			var tournamentId = new TournamentId();
			var eventEntryId = Guid.NewGuid();
			var playersNames = new List<string>();
			for (int i = 0; i < numberOfPlayers; i++)
			{
				playersNames.Add("Player");
			}

			Action act = () => Competitor.Create(tournamentId, eventType, eventEntryId, 1,
				playersNames);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Wrong number of players for this event type");
		}
	}
}
