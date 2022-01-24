using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
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
			tournament.Venue.Name.Should().Be("All England Lawn Tennis Club");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateTournamentWithEmptyTitle(string title)
		{
			var venue = Venue.Create(new VenueId(), "Roland Garros", Surface.Clay);
			Action act = () => Tournament.Create(title, TournamentLevel.Masters125,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14), venue);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(title == null
					? "Value cannot be null. (Parameter 'title')"
					: "Required input title was empty. (Parameter 'title')");
		}

		[Fact]
		public void CannotCreateTournamentWithNullVenue()
		{
			Action act = () => Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14), null);

			act.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'venue')");
		}

		[Fact]
		public void CanUpdateTournamentDetails()
		{
			var tournament = CreateTestTournament();

			var venue = Venue.Create(new VenueId(), "Roland Garros", Surface.Clay);
			tournament.UpdateDetails("New Wimbledon", TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17), venue);

			tournament.Title.Should().Be("New Wimbledon");
			tournament.Level.Should().Be(TournamentLevel.Masters500);
			tournament.State.Should().Be(TournamentState.BeingDefined);
			tournament.StartDate.Should().Be(new DateTime(2019, 7, 4));
			tournament.EndDate.Should().Be(new DateTime(2019, 7, 17));
			tournament.Venue.Name.Should().Be(venue.Name);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotUpdateTournamentToHaveEmptyTitle(string title)
		{
			var tournament = CreateTestTournament();
			var venue = Venue.Create(new VenueId(), "Roland Garros", Surface.Clay);

			Action act = () => tournament.UpdateDetails(title, TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17), venue);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(title == null
					? "Value cannot be null. (Parameter 'title')"
					: "Required input title was empty. (Parameter 'title')");
		}

		[Fact]
		public void CannotUpdateTournamentDetailsToHaveNoVenue()
		{
			var tournament = CreateTestTournament();

			Action act = () => tournament.UpdateDetails("New Wimbledon", TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17), null);

			act.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'venue')");
		}

		[Fact]
		public void CanAddAnEventToATournament()
		{
			var tournament = CreateTestTournament();
			var tennisEvent = CreateTestEvent();

			tournament.AddEvent(tennisEvent);

			tournament.Events.Count.Should().Be(1);
			tournament.Events[0].EventSize.EntrantsLimit.Should().Be(128);
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
			tournament.Events[0].EventType.Should().Be(EventType.WomensSingles);
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
		public void CannotRemoveAnEventFromATournamentIfTheEventDoesNotExist()
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
		public void CanClearAllEventsForTheTournament()
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
			var venue = Venue.Create(new VenueId(), "Roland Garros", Surface.Clay);

			void act() => tournament.UpdateDetails("New Wimbledon", TournamentLevel.Masters500,
				new DateTime(2019, 7, 4), new DateTime(2019, 7, 17), venue);

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
		public void CannotEnterAnEventIfTournamentIsNotInAcceptingEntriesState()
		{
			var tournament = CreateTestTournament();
			var player = Player.Register(new PlayerId(), "Peter Player", 10, 200, Gender.Male);

			void act() => tournament.EnterEvent(EventType.MensSingles, player);

			VerifyExceptionThrownWhenNotInCorrectState(act, "EnterEvent", tournament.State);
		}

		[Fact]
		public void CannotEnterAnEventIfTheTournamentDoesNotHaveThatTypeOfEvent()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();
			var player = Player.Register(new PlayerId(), "Paula Player", 10, 200, Gender.Female);

			Action act = () => tournament.EnterEvent(EventType.WomensSingles, player);

			act.Should().Throw<Exception>()
				.WithMessage("Tournament does not have an event of type WomensSingles");
		}

		[Fact]
		public void CanEnterAnSinglesEventWithOnePlayer()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();
			var player = Player.Register(new PlayerId(), "Peter Player", 10, 200, Gender.Male);

			tournament.EnterEvent(EventType.MensSingles, player);

			tournament.Events[0].Entries[0].PlayerOne.Name.Should().Be("Peter Player");
			tournament.Events[0].Entries[0].PlayerTwo.Should().BeNull();
		}

		[Fact]
		public void CanEnterAnDoublesEventWithTwoPlayers()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32, MatchFormat.ThreeSetMatchWithFinalSetTieBreak));
			tournament.AddEvent(Event.Create(EventType.MensDoubles, 64, 16, MatchFormat.ThreeSetMatchWithFinalSetTieBreak));
			tournament.OpenForEntries();

			var playerOne = Player.Register(new PlayerId(), "Peter Player", 10, 200, Gender.Male);
			var playerTwo = Player.Register(new PlayerId(), "Steve Serve", 15, 100, Gender.Male);

			tournament.EnterEvent(EventType.MensDoubles, playerOne, playerTwo);

			tournament.Events[1].Entries[0].PlayerOne.Name.Should().Be("Peter Player");
			tournament.Events[1].Entries[0].PlayerTwo.Name.Should().Be("Steve Serve");
		}

		[Fact]
		public void PlayerCanWithdrawFromSinglesEvent()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();
			var playerOne = Player.Register(new PlayerId(), "Peter Player", 10, 200, Gender.Male);
			var playerTwo = Player.Register(new PlayerId(), "Steve Serve", 15, 100, Gender.Male);

			tournament.EnterEvent(EventType.MensSingles, playerOne);
			tournament.EnterEvent(EventType.MensSingles, playerTwo);

			tournament.WithdrawFromEvent(EventType.MensSingles, playerTwo);

			tournament.Events[0].Entries.Count.Should().Be(1);
			tournament.Events[0].Entries.Any(e => e.PlayerOne.Id == playerTwo.Id).Should().BeFalse();
			tournament.Events[0].Entries.Any(e => e.PlayerOne.Id == playerOne.Id).Should().BeTrue();
		}

		[Fact]
		public void PairOfPlayersCanWithdrawFromDoublesEvent()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(Event.Create(EventType.MensDoubles, 128, 32, MatchFormat.ThreeSetMatchWithFinalSetTieBreak));
			tournament.OpenForEntries();
			var playerOne = Player.Register(new PlayerId(), "Peter Player", 10, 200, Gender.Male);
			var playerTwo = Player.Register(new PlayerId(), "Steve Serve", 15, 100, Gender.Male);

			tournament.EnterEvent(EventType.MensDoubles,
				Player.Register(new PlayerId(), "John", 10, 200, Gender.Male),
				Player.Register(new PlayerId(), "John", 11, 201, Gender.Male));

			tournament.EnterEvent(EventType.MensDoubles, playerOne, playerTwo);

			tournament.WithdrawFromEvent(EventType.MensDoubles, playerOne, playerTwo);

			tournament.Events[0].Entries.Count.Should().Be(1);
			tournament.Events[0].Entries
				.Any(e => e.PlayerOne.Id == playerOne.Id && e.PlayerTwo.Id == playerTwo.Id)
				.Should().BeFalse();
		}

		[Fact]
		public void CannotWithdrawPlayersIfTournamentIsNotInOpenForEntriesState()
		{
			var tournament = CreateTestTournament();
			tournament.AddEvent(CreateTestEvent(EventType.MensSingles));
			var player = Player.Register(new PlayerId(), "Steve Serve", 1, 1, Gender.Male);

			Action act = () => tournament.WithdrawFromEvent(EventType.MensSingles, player);

			act.Should().Throw<Exception>()
				.WithMessage("Action WithdrawFromEvent not allowed for a tournament in the state BeingDefined");
		}

		[Fact]
		public void CannotWithdrawFromAnEventIfTheTournamentDoeNotHaveAnEventOfThatType()
		{
			var tournament = CreateTestTournamentAndOpenForEntries();
			var player = Player.Register(new PlayerId(), "Steve Serve", 1, 1, Gender.Male);

			Action act = () => tournament.WithdrawFromEvent(EventType.WomensSingles, player);

			act.Should().Throw<Exception>()
				.WithMessage("Tournament does not have an event of type WomensSingles");
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
			var venue = Venue.Create(new VenueId(), "All England Lawn Tennis Club", Surface.Clay);

			return Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14), venue);
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
