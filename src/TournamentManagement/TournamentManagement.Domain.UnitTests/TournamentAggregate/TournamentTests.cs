using FluentAssertions;
using System;
using System.Collections.Generic;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateTournamentAndItIsCreatedCorrectly()
		{
			var tournament = CreateTestTournament();

			tournament.Id.Id.Should().NotBe(Guid.Empty);
			tournament.Title.Should().Be("Wimbledon");
			tournament.Year.Should().Be(2019);
			tournament.Level.Should().Be(TournamentLevel.GrandSlam);
			tournament.State.Should().Be(TournamentState.BeingDefined);
			tournament.StartDate.Should().Be(new DateTime(2019, 7, 1));
			tournament.EndDate.Should().Be(new DateTime(2019, 7, 14));
			tournament.VenueId.Id.Should().NotBe(Guid.Empty);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateTournamentWithEmptyTitle(string title)
		{
			Action act = () => Tournament.Create(title, TournamentLevel.Masters125,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14), new VenueId());

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(title == null
					? "Value cannot be null. (Parameter 'title')"
					: "Required input title was empty. (Parameter 'title')");
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
		public void CannnotAddASecondEventOfTheSameTypeToATournament()
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
			var tournament = CreateTestTournament();

			var events = new List<Event>()
			{
				CreateTestEvent(EventType.MensSingles),
				CreateTestEvent(EventType.WomensSingles),
				CreateTestEvent(EventType.MensDoubles),
				CreateTestEvent(EventType.WomensDoubles),
				CreateTestEvent(EventType.MixedDoubles)
			};

			

			tournament.SetEvents(events);

			tournament.Events.Count.Should().Be(5);
		}

		[Fact]
		public void CannotAddACollectionOfEventsToATournammentIfTwoHaveTheSameType()
		{
			var tournament = CreateTestTournament();

			var events = new List<Event>()
			{
				CreateTestEvent(EventType.MensSingles),
				CreateTestEvent(EventType.MensSingles)
			};

			Action act = () => tournament.SetEvents(events);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament already has an event of type MensSingles");

			tournament.Events.Count.Should().Be(0);
		}

		[Fact]
		public void CanTransitionThroughTheStatesOfATournament()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			tournament.AddEvent(CreateTestEvent(EventType.WomensSingles));

			tournament.OpenForEntries();
			tournament.State.Should().Be(TournamentState.AcceptingEntries);

			tournament.CloseEntries();
			tournament.State.Should().Be(TournamentState.EntriesClosed);

			tournament.DrawTheEvents();
			tournament.State.Should().Be(TournamentState.DrawComplete);

			tournament.StartTournament();
			tournament.State.Should().Be(TournamentState.InProgress);

			tournament.EventCompleted(EventType.MensSingles);
			tournament.State.Should().Be(TournamentState.InProgress);

			tournament.EventCompleted(EventType.WomensSingles);
			tournament.State.Should().Be(TournamentState.Complete);
		}

		[Fact]
		public void CannotUpdateTournamentDetailsIfItIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();

			void act() => tournament.UpdateDetails("New Wimbledon", TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17));

			VerifyExceptionThrownWhenNotInCorrectState(act, "UpdateDetails", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotAddEventIfTournamentIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();
			var tennisEvent = CreateTestEvent(EventType.WomensSingles);

			void act() => tournament.AddEvent(tennisEvent);

			VerifyExceptionThrownWhenNotInCorrectState(act, "AddEvent", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotRemoveEventIfTournamentIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();

			void act() => tournament.RemoveEvent(EventType.MensSingles);

			VerifyExceptionThrownWhenNotInCorrectState(act, "RemoveEvent", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotClearEventsIfTournamentIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();

			void act() => tournament.ClearEvents();

			VerifyExceptionThrownWhenNotInCorrectState(act, "ClearEvents", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotSetEventsIfTournamentIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();

			void act() => tournament.SetEvents(null);

			VerifyExceptionThrownWhenNotInCorrectState(act, "SetEvents", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotOpenForEntriesIfTournamentIsNotInBeingDefinedState()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();

			void act() => tournament.OpenForEntries();

			VerifyExceptionThrownWhenNotInCorrectState(act, "OpenForEntries", TournamentState.AcceptingEntries);
		}

		[Fact]
		public void CannotOpenForEntriesIfTournamentHasNoEvents()
		{
			var tournament = CreateTestTournament();

			Action act = () => tournament.OpenForEntries();

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Tournament must have at least one event to open it for entries");
		}

		[Fact]
		public void CannotCloseEntriesIfTournamentIsNotInAcceptingEntriesState()
		{
			var tournament = CreateTestTournament();

			void act() => tournament.CloseEntries();

			VerifyExceptionThrownWhenNotInCorrectState(act, "CloseEntries", tournament.State);
		}

		[Fact]
		public void CannotDrawTheEventsIfTournamentIsNotInEntriesClosedState()
		{
			var tournament = CreateTestTournament();

			void act() => tournament.DrawTheEvents();

			VerifyExceptionThrownWhenNotInCorrectState(act, "DrawTheEvents", tournament.State);
		}

		[Fact]
		public void CannotStartTournamentIfTournamentIsNotInDrawCompleteState()
		{
			var tournament = CreateTestTournament();

			void act() => tournament.StartTournament();

			VerifyExceptionThrownWhenNotInCorrectState(act, "StartTournament", tournament.State);
		}

		[Fact]
		public void CannotCompleteAnEventIfTournamentIsNotInInProgressState()
		{
			var tournament = CreateTestTournament();

			void act() => tournament.EventCompleted(EventType.MensSingles);

			VerifyExceptionThrownWhenNotInCorrectState(act, "EventCompleted", tournament.State);
		}

		private static Tournament CreateTestTournament()
		{
			return Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14), new VenueId());
		}

		private static Event CreateTestEvent(EventType eventtype = EventType.MensSingles)
		{
			return Event.Create(eventtype, 128, 32, MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
		}

		private static Tournament CreateTestTournamentAndOpenForEntries()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			tournament.OpenForEntries();
			return tournament;
		}

		private static void VerifyExceptionThrownWhenNotInCorrectState(Action act, string action, TournamentState state)
		{
			act.Should()
				.Throw<Exception>()
				.WithMessage($"Action {action} not allowed for a tournament in the state {state}");
		}
	}
}
