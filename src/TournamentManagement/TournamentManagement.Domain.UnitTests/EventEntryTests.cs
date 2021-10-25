using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class EventEntryTests
	{
		[Theory]
		[InlineData(EventType.MensSingles)]
		[InlineData(EventType.WomensSingles)]
		public void CanUseFactoryMethodToCreateEntryToSinglesEvent(EventType eventType)
		{
			var tournamentId = Guid.NewGuid();
			var player = Player.Create("Steve Server", 20, 100, Gender.Male);

			var entry = EventEntry.CreateSinglesEntry(tournamentId, eventType, player);

			entry.Id.Should().NotBe(Guid.Empty);
			entry.TournamentId.Should().Be(tournamentId);
			entry.EventType.Should().Be(eventType);
			entry.Players.Count.Should().Be(1);
			entry.Rank.Should().Be(player.SinglesRank);
		}

		[Theory]
		[InlineData(EventType.MensDoubles)]
		[InlineData(EventType.WomensDoubles)]
		[InlineData(EventType.MixedDoubles)]
		public void CanUseFactoryMethodToCreateEntryToDoublesEvent(EventType eventType)
		{
			var tournamentId = Guid.NewGuid();
			var playerOne = Player.Create("Steve Server", 20, 100, Gender.Male);
			var playerTwo = Player.Create("Gary Groundstroke", 30, 50, Gender.Male);

			var entry = EventEntry.CreateDoublesEntry(tournamentId, eventType, playerOne, playerTwo);

			entry.Id.Should().NotBe(Guid.Empty);
			entry.TournamentId.Should().Be(tournamentId);
			entry.EventType.Should().Be(eventType);
			entry.Players.Count.Should().Be(2);
			entry.Rank.Should().Be(playerTwo.DoublesRank);
		}

		[Theory]
		[InlineData(EventType.MensSingles)]
		[InlineData(EventType.WomensSingles)]
		public void CannotCreateSinglesEntryForADoublesEventType(EventType eventType)
		{
			var tournamentId = Guid.NewGuid();
			var playerOne = Player.Create("Steve Server", 20, 100, Gender.Male);
			var playerTwo = Player.Create("Gary Groundstroke", 30, 50, Gender.Male);

			Action act = () => EventEntry.CreateDoublesEntry(tournamentId, eventType, playerOne, playerTwo);

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
			var tournamentId = Guid.NewGuid();
			var player = Player.Create("Steve Server", 20, 100, Gender.Male);

			Action act = () => EventEntry.CreateSinglesEntry(tournamentId, eventType, player);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"{eventType} is not a singles event");
		}

		[Fact]
		public void TheTournamentIdCannotBeAnEmptyGuid()
		{
			var tournamentId = Guid.Empty;
			var player = Player.Create("Steve Server", 20, 100, Gender.Male);

			Action act = () => EventEntry.CreateSinglesEntry(tournamentId, EventType.MensSingles, player);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'tournamentId')");
		}
	}
}
