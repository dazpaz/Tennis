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
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

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
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

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
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			tournament.AddEvent(tennisEvent);

			tournament.Events.Count.Should().Be(1);
			tournament.Events[EventType.MensSingles].EventSize.EntrantsLimit.Should().Be(128);
		}

		[Fact]
		public void CanAddAllTypesOfEventToATournament()
		{
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithTwoGamesClear));
			tournament.AddEvent(Event.Create(EventType.MensDoubles, 64, 16,
				MatchFormat.FiveSetMatchWithFinalSetTieBreak));
			tournament.AddEvent(Event.Create(EventType.WomensDoubles, 64, 16,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak));
			tournament.AddEvent(Event.Create(EventType.MixedDoubles, 32, 8,
				new MatchFormat(3, FinalSetType.ChampionsTieBreak)));

			tournament.Events.Count.Should().Be(5);
		}

		[Fact]
		public void CanRemoveAnEventFromATournament()
		{
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 64, 16,
				MatchFormat.ThreeSetMatchWithTwoGamesClear));

			tournament.Events.Count.Should().Be(2);

			tournament.RemoveEvent(EventType.MensSingles);

			tournament.Events.Count.Should().Be(1);
			tournament.Events[EventType.WomensSingles].EventSize.EntrantsLimit.Should().Be(64);
		}

		[Fact]
		public void CannnotAddASecondEventIfTheSameTypeToATournament()
		{
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));
			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));

			Action act = () => tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament already has an event of type MensSingles");
		}

		[Fact]
		public void CannotRemoveAnEventTypeFromATournamentIfItDoesNotExist()
		{
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));

			tournament.Events.Count.Should().Be(1);

			Action act = () => tournament.RemoveEvent(EventType.WomensSingles);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament does not have an event of type WomensSingles");
		}

		[Fact]
		public void CanClearTheEventsForTheTournament()
		{
			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.FiveSetMatchWithTwoGamesClear));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithTwoGamesClear));

			tournament.Events.Count.Should().Be(2);

			tournament.ClearEvents();

			tournament.Events.Count.Should().Be(0);
		}

		[Fact]
		public void CanAddACollectionOfEventsToATournamment()
		{
			var events = new List<Event>()
			{
				Event.Create(EventType.MensSingles, 128, 32,
					MatchFormat.FiveSetMatchWithTwoGamesClear),
				Event.Create(EventType.WomensSingles, 128, 32,
					MatchFormat.FiveSetMatchWithTwoGamesClear),
				Event.Create(EventType.MensDoubles, 64, 16,
					MatchFormat.FiveSetMatchWithFinalSetTieBreak),
				Event.Create(EventType.WomensDoubles, 64, 16,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak),
					Event.Create(EventType.MixedDoubles, 32, 8,
				new MatchFormat(3, FinalSetType.ChampionsTieBreak))
			};

			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.SetEvents(events);

			tournament.Events.Count.Should().Be(5);
		}

		[Fact]
		public void CannotAddACollectionOfEventsToATournammentIfTwoHaveTheSameType()
		{
			var events = new List<Event>()
			{
				Event.Create(EventType.MensSingles, 128, 32,
					MatchFormat.FiveSetMatchWithTwoGamesClear),
				Event.Create(EventType.MensSingles, 128, 32,
					MatchFormat.FiveSetMatchWithTwoGamesClear)
			};

			var tournament = Tournament.Create("Wimbledon", TournamentLevel.GrandSlam,
				new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			Action act = () => tournament.SetEvents(events);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Tournament already has an event of type MensSingles");

			tournament.Events.Count.Should().Be(0);
		}
	}
}
