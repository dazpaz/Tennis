using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class TournamentTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateTournamentAndItIsCreatedCorrectly()
		{
			var tournament = CreateTestTournament();

			tournament.Id.Should().NotBe(Guid.Empty);
			tournament.Title.Should().Be("Wimbledon");
			tournament.Year.Should().Be(2019);
			tournament.Level.Should().Be(TournamentLevel.GrandSlam);
			tournament.State.Should().Be(TournamentState.BeingDefined);
			tournament.StartDate.Should().Be(new DateTime(2019, 7, 1));
			tournament.EndDate.Should().Be(new DateTime(2019, 7, 14));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotCreateTournamentWithEmptyTitle(string title)
		{
			Action act = () => Tournament.Create(title, TournamentLevel.Masters125,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'title')");
		}

		[Fact]
		public void CanUpdateTournamentDetails()
		{
			var tournament = CreateTestTournament();

			tournament.UpdateDetails("New Wimbledon", TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17));

			tournament.Title.Should().Be("New Wimbledon");
			tournament.Level.Should().Be(TournamentLevel.Masters500);
			tournament.State.Should().Be(TournamentState.BeingDefined);
			tournament.StartDate.Should().Be(new DateTime(2019, 7, 4));
			tournament.EndDate.Should().Be(new DateTime(2019, 7, 17));
		}

		[Fact]
		public void CanAddAnEventToATournament()
		{
			var tournament = CreateTestTournament();
			var tennisEvent = CreateTestEvent();

			tournament.AddEvent(tennisEvent);

			tournament.Events.Count.Should().Be(1);
			tournament.Events[EventType.MensSingles].EventSize.EntrantsLimit.Should().Be(128);
		}

		[Fact]
		public void CanAddAllTypesOfEventToATournament()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			tournament.AddEvent(CreateTestEvent(EventType.WomensSingles));
			tournament.AddEvent(CreateTestEvent(EventType.MensDoubles));
			tournament.AddEvent(CreateTestEvent(EventType.WomensDoubles));
			tournament.AddEvent(CreateTestEvent(EventType.MixedDoubles));

			tournament.Events.Count.Should().Be(5);
		}

		[Fact]
		public void CanRemoveAnEventFromATournament()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			tournament.AddEvent(CreateTestEvent(EventType.WomensSingles));

			tournament.Events.Count.Should().Be(2);

			tournament.RemoveEvent(EventType.MensSingles);

			tournament.Events.Count.Should().Be(1);
			tournament.Events[EventType.WomensSingles].EventSize.EntrantsLimit.Should().Be(128);
		}

		[Fact]
		public void CannnotAddASecondEventIfTheSameTypeToATournament()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));

			Action act = () => tournament.AddEvent(CreateTestEvent(EventType.MensSingles));

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament already has an event of type MensSingles");
		}

		[Fact]
		public void CannotRemoveAnEventTypeFromATournamentIfItDoesNotExist()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));

			tournament.Events.Count.Should().Be(1);

			Action act = () => tournament.RemoveEvent(EventType.WomensSingles);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament does not have an event of type WomensSingles");
		}

		[Fact]
		public void CanClearTheEventsForTheTournament()
		{
			var tournament = CreateTestTournament();

			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			tournament.AddEvent(CreateTestEvent(EventType.WomensSingles));

			tournament.Events.Count.Should().Be(2);

			tournament.ClearEvents();

			tournament.Events.Count.Should().Be(0);
		}

		[Fact]
		public void CanAddACollectionOfEventsToATournamment()
		{
			var events = new List<Event>()
			{
				CreateTestEvent(EventType.MensSingles),
				CreateTestEvent(EventType.WomensSingles),
				CreateTestEvent(EventType.MensDoubles),
				CreateTestEvent(EventType.WomensDoubles),
				CreateTestEvent(EventType.MixedDoubles)
			};

			var tournament = CreateTestTournament();

			tournament.SetEvents(events);

			tournament.Events.Count.Should().Be(5);
		}

		[Fact]
		public void CannotAddACollectionOfEventsToATournammentIfTwoHaveTheSameType()
		{
			var events = new List<Event>()
			{
				CreateTestEvent(EventType.MensSingles),
				CreateTestEvent(EventType.MensSingles)
			};

			var tournament = CreateTestTournament();

			Action act = () => tournament.SetEvents(events);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament already has an event of type MensSingles");

			tournament.Events.Count.Should().Be(0);
		}

		private static Tournament CreateTestTournament()
		{
			return Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));
		}

		private static Event CreateTestEvent(EventType eventtype = EventType.MensSingles)
		{
			return Event.Create(eventtype, 128, 32, MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
		}
	}
}
