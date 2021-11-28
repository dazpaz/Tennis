using FluentAssertions;
using System;
using System.Collections.Generic;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.CompetitorAggregate
{
	public class CompetitorTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForASinglesEventAndItIsCreatedCorrectly()
		{
			var tournamentId = new TournamentId();
			var eventEntryId = new EventEntryId();
			var seeding = new Seeding(1);
			var competitor = Competitor.Create(tournamentId, EventType.MensSingles, eventEntryId,
				seeding, new List<string>() { "Steve Serve" });

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.TournamentId.Should().Be(tournamentId);
			competitor.EventType.Should().Be(EventType.MensSingles);
			competitor.EventEntryId.Should().Be(eventEntryId);
			competitor.Seeding.Should().Be(seeding);
			competitor.PlayerNames.Count.Should().Be(1);
			competitor.PlayerNames[0].Should().Be("Steve Serve");
		}

		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForADoublesEventAndItIsCreatedCorrectly()
		{
			var tournamentId = new TournamentId();
			var eventEntryId = new EventEntryId();
			var seeding = new Seeding(1);
			var competitor = Competitor.Create(tournamentId, EventType.MensDoubles, eventEntryId,
				seeding, new List<string>() { "Steve Serve", "Vernon Volley" });

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.TournamentId.Should().Be(tournamentId);
			competitor.EventType.Should().Be(EventType.MensDoubles);
			competitor.EventEntryId.Should().Be(eventEntryId);
			competitor.Seeding.Should().Be(seeding);
			competitor.PlayerNames.Count.Should().Be(2);
			competitor.PlayerNames[0].Should().Be("Steve Serve");
			competitor.PlayerNames[1].Should().Be("Vernon Volley");
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
			var eventEntryId = new EventEntryId();
			var expectedPlayerCount = numberOfPlayers == 2 ? 1 : 2;
			var playersNames = new List<string>();
			for (int i = 0; i < numberOfPlayers; i++)
			{
				playersNames.Add("Player");
			}

			Action act = () => Competitor.Create(tournamentId, eventType, eventEntryId, new Seeding(1),
				playersNames);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Competitor for {eventType} event must have {expectedPlayerCount} players");
		}
	}
}
