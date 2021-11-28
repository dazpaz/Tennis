using FluentAssertions;
using System;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class EventEntryTests
	{
		[Theory]
		[InlineData(EventType.MensSingles, Gender.Male)]
		[InlineData(EventType.WomensSingles, Gender.Female)]
		public void CanUseFactoryMethodToCreateEntryToSinglesEvent(EventType eventType, Gender gender)
		{
			var tournamentId = new TournamentId();
			var playerId = new PlayerId();
			var player = Player.Create(playerId, "Steve Server", 20, 100, gender);

			var entry = EventEntry.CreateSinglesEventEntry(tournamentId, eventType, player);

			entry.Id.Should().NotBe(Guid.Empty);
			entry.TournamentId.Id.Should().Be(tournamentId.Id);
			entry.EventType.Should().Be(eventType);
			entry.Players.Count.Should().Be(1);
			entry.Rank.Should().Be(player.SinglesRank);
		}

		[Theory]
		[InlineData(EventType.MensDoubles, Gender.Male, Gender.Male)]
		[InlineData(EventType.WomensDoubles, Gender.Female, Gender.Female)]
		[InlineData(EventType.MixedDoubles, Gender.Male, Gender.Female)]
		public void CanUseFactoryMethodToCreateEntryToDoublesEvent(EventType eventType,
			Gender genderOne, Gender genderTwo)
		{
			var tournamentId = new TournamentId();
			var playerOne = Player.Create(new PlayerId(), "Steve Server", 20, 100, genderOne);
			var playerTwo = Player.Create(new PlayerId(), "Gary Groundstroke", 30, 50, genderTwo);

			var entry = EventEntry.CreateDoublesEventEntry(tournamentId, eventType, playerOne, playerTwo);

			entry.Id.Should().NotBe(Guid.Empty);
			entry.TournamentId.Id.Should().Be(tournamentId.Id);
			entry.EventType.Should().Be(eventType);
			entry.Players.Count.Should().Be(2);
			entry.Rank.Should().Be(playerTwo.DoublesRank);
		}

		[Theory]
		[InlineData(EventType.MensSingles)]
		[InlineData(EventType.WomensSingles)]
		public void CannotCreateSinglesEntryForADoublesEventType(EventType eventType)
		{
			var tournamentId = new TournamentId();
			var playerOne = Player.Create(new PlayerId(), "Steve Server", 20, 100, Gender.Male);
			var playerTwo = Player.Create(new PlayerId(), "Gary Groundstroke", 30, 50, Gender.Male);

			Action act = () => EventEntry.CreateDoublesEventEntry(tournamentId, eventType, playerOne, playerTwo);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"{eventType} is not a doubles event");
		}

		[Theory]
		[InlineData(EventType.MensDoubles)]
		[InlineData(EventType.WomensDoubles)]
		[InlineData(EventType.MixedDoubles)]
		public void CannotCreateDoublesEntryToSinglesEvent(EventType eventType)
		{
			var tournamentId = new TournamentId();
			var player = Player.Create(new PlayerId(), "Steve Server", 20, 100, Gender.Male);

			Action act = () => EventEntry.CreateSinglesEventEntry(tournamentId, eventType, player);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"{eventType} is not a singles event");
		}

		[Theory]
		[InlineData(EventType.MensSingles, Gender.Female)]
		[InlineData(EventType.WomensSingles, Gender.Male)]
		public void IfGenderDoesNotMatchSinglesEventThenExceptionIsThrown(EventType eventType, Gender gender)
		{
			var tournamentId = new TournamentId();
			var player = Player.Create(new PlayerId(), "Steve Server", 20, 100, gender);

			Action act = () => EventEntry.CreateSinglesEventEntry(tournamentId, eventType, player);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Gender of players does not match the event type {eventType}");
		}

		[Theory]
		[InlineData(EventType.MensDoubles, Gender.Male, Gender.Female)]
		[InlineData(EventType.WomensDoubles, Gender.Male, Gender.Female)]
		[InlineData(EventType.MixedDoubles, Gender.Male, Gender.Male)]
		public void IfGenderDoesNotMatchDoubleEventThenExceptionIsThrown(EventType eventType,
			Gender genderOne, Gender genderTwo)
		{
			var tournamentId = new TournamentId();
			var playerOne = Player.Create(new PlayerId(), "Steve Server", 20, 100, genderOne);
			var playerTwo = Player.Create(new PlayerId(), "Gary Groundstroke", 30, 50, genderTwo);

			Action act = () => EventEntry.CreateDoublesEventEntry(tournamentId, eventType, playerOne, playerTwo);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Gender of players does not match the event type {eventType}");
		}
	}
}
