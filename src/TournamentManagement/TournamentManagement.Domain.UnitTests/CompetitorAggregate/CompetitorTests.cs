using FluentAssertions;
using System;
using System.Collections.Generic;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.CompetitorAggregate
{
	public class CompetitorTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForASinglesEventAndItIsCreatedCorrectly()
		{
			var tournament = CreateTestTournament();
			var seeding = new Seeding(1);
			var competitor = Competitor.Create(tournament, EventType.MensSingles, seeding, "Steve Serve");

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.Tournament.Should().Be(tournament);
			competitor.EventType.Should().Be(EventType.MensSingles);
			competitor.Seeding.Should().Be(seeding);
			competitor.PlayerOneName.Should().Be("Steve Serve");
			competitor.PlayerTwoName.Should().BeNull();
		}

		[Fact]
		public void CanUseFactoryMethodToCreateCompetitorForADoublesEventAndItIsCreatedCorrectly()
		{
			var tournament = CreateTestTournament();
			var seeding = new Seeding(1);
			var competitor = Competitor.Create(tournament, EventType.MensDoubles,
				seeding, "Steve Serve", "Vernon Volley");

			competitor.Id.Id.Should().NotBe(Guid.Empty);
			competitor.Tournament.Should().Be(tournament);
			competitor.EventType.Should().Be(EventType.MensDoubles);
			competitor.Seeding.Should().Be(seeding);
			competitor.PlayerOneName.Should().Be("Steve Serve");
			competitor.PlayerTwoName.Should().Be("Vernon Volley");
		}

		[Fact]
		public void CannotCreateCompetitorWithANullTournament()
		{
			Action act = () => Competitor.Create(null, EventType.MensSingles, null, "Steve Serve");

			act.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'tournament')");
		}

		[Fact]
		public void CannotCreateCompetitorWithANullSeeding()
		{
			var tournament = CreateTestTournament();
			Action act = () => Competitor.Create(tournament, EventType.MensSingles, null, "Steve Serve");

			act.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'seeding')");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateACompetitorWithEmptyPlayerOneName(string name)
		{
			var tournament = CreateTestTournament();
			var seeding = new Seeding(1);

			Action act = () => Competitor.Create(tournament, EventType.MensSingles, seeding, name);

			act.Should().Throw<ArgumentException>()
				.WithMessage(name == null
				? "Value cannot be null. (Parameter 'playerOneName')"
				: "Required input playerOneName was empty. (Parameter 'playerOneName')");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateACompetitorWithEmptyPlayerTwoNameForDoublesEvent(string name)
		{
			var tournament = CreateTestTournament();
			var seeding = new Seeding(1);

			Action act = () => Competitor.Create(tournament, EventType.MensDoubles, seeding, "Steve Serve", name);

			act.Should().Throw<ArgumentException>()
				.WithMessage(name == null
				? "Value cannot be null. (Parameter 'playerTwoName')"
				: "Required input playerTwoName was empty. (Parameter 'playerTwoName')");
		}

		private static Tournament CreateTestTournament()
		{
			var venue = Venue.Create(new VenueId(), "AETLA", Surface.Grass);
			return Tournament.Create("Wimbledon", TournamentLevel.Masters500,
				DateTime.Today, DateTime.Today, venue);
		}
	}
}
