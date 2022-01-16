﻿using FluentAssertions;
using System;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class EventTests
	{
		[Fact]
		public void CanCreateAnEventUsingTheFactory()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			tennisEvent.Id.Id.Should().NotBe(Guid.Empty);
			tennisEvent.IsCompleted.Should().BeFalse();
			tennisEvent.EventType.Should().Be(EventType.MensSingles);
			tennisEvent.MatchFormat.Should().Be(MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
			tennisEvent.EventSize.EntrantsLimit.Should().Be(128);
			tennisEvent.EventSize.NumberOfSeeds.Should().Be(32);
		}

		[Theory]
		[InlineData(EventType.MensDoubles, false)]
		[InlineData(EventType.MensSingles, true)]
		[InlineData(EventType.MixedDoubles, false)]
		[InlineData(EventType.WomensDoubles, false)]
		[InlineData(EventType.WomensSingles, true)]
		public void TheIsSinglesEventPropertyIsSetCorrectlyBasedOnEventType(EventType eventType, bool isSingles)
		{
			var tennisEvent = Event.Create(eventType, 128, 32, new MatchFormat(1, SetType.TieBreak));

			tennisEvent.SinglesEvent.Should().Be(isSingles);
		}

		[Fact]
		public void CanUpdateAnEvent()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			var id = tennisEvent.Id;

			tennisEvent.UpdateDetails(EventType.MensDoubles, 64, 16,
				MatchFormat.OneSetMatchWithFinalSetTieBreak);

			tennisEvent.Id.Should().Be(id);
			tennisEvent.EventType.Should().Be(EventType.MensDoubles);
			tennisEvent.MatchFormat.Should().Be(MatchFormat.OneSetMatchWithFinalSetTieBreak);
			tennisEvent.EventSize.EntrantsLimit.Should().Be(64);
			tennisEvent.EventSize.NumberOfSeeds.Should().Be(16);
		}

		[Fact]
		public void CanMarkAnEventAsCompleted()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			tennisEvent.IsCompleted.Should().BeFalse();

			tennisEvent.MarkEventCompleted();

			tennisEvent.IsCompleted.Should().BeTrue();
		}

		[Fact]
		public void CannotUpdateAnEventThatIsCompleted()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
			tennisEvent.MarkEventCompleted();

			Action act = () => tennisEvent.UpdateDetails(EventType.WomensSingles, 64, 16,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Cannot update the details of an event that is completed");
		}

		[Fact]
		public void CanEnterASinglesEvent()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
			var player = Player.Register(new PlayerId(), "Steve", 100, 50, Gender.Male);

			tennisEvent.Enter(player);

			tennisEvent.Entries.Count.Should().Be(1);
			tennisEvent.Entries[0].PlayerOne.Should().Be(player);
			tennisEvent.Entries[0].PlayerTwo.Should().BeNull();
		}

		[Fact]
		public void CanRemoveAnEntryFromAnEvent()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			var player = Player.Register(new PlayerId(), "Steve", 100, 50, Gender.Male);

			tennisEvent.Enter(player);
			player = Player.Register(new PlayerId(), "Dave", 101, 52, Gender.Male);
			var entryId = tennisEvent.Enter(player);
			tennisEvent.Entries.Count.Should().Be(2);

			tennisEvent.RemoveEntry(entryId);

			tennisEvent.Entries.Count.Should().Be(1);
		}

		[Fact]
		public void CanClearAllEntriesFromAnEvent()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			var player = Player.Register(new PlayerId(), "Steve", 100, 50, Gender.Male);
			tennisEvent.Enter(player);
			player = Player.Register(new PlayerId(), "Dave", 101, 52, Gender.Male);
			tennisEvent.Enter(player);
			tennisEvent.Entries.Count.Should().Be(2);

			tennisEvent.ClearEntries();

			tennisEvent.Entries.Count.Should().Be(0);
		}
	}
}
