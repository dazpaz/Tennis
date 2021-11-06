using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class MatchTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateMatchAndItIsCreatedCorrectly()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.Id.Id.Should().NotBe(Guid.Empty);
			match.Format.Should().Be(MatchFormat.OneSetMatchWithTwoGamesClear);
			match.State.Should().Be(MatchState.Created);
			match.Outcome.Should().Be(MatchOutcome.AwaitingOutcome);

			match.Competitors.Count.Should().Be(2);
			match.Competitors[0].Should().BeNull();
			match.Competitors[1].Should().BeNull();

			match.SetScores.Count.Should().Be(0);

			match.Court.Should().BeNull();
			match.Winner.Should().BeNull();
		}

		[Fact]
		public void CanAddACompetitorInPositionOneOfTheMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor, 1);

			match.Competitors[0].Should().Be(competitor);
			match.Competitors[1].Should().BeNull();
		}

		[Fact]
		public void CanAddACompetitorInPositionTwoOfTheMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor, 2);

			match.Competitors[0].Should().BeNull();
			match.Competitors[1].Should().Be(competitor);
			
		}

		[Fact]
		public void CanAddBothCompetitorsToTheMatch()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Competitors[0].Should().Be(competitor1);
			match.Competitors[1].Should().Be(competitor2);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		public void CannotAddACompetitorIntoAnInvalidPosition(int position)
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			Action act = () => match.AddCompetitor(competitor, position);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Invalid Competitor Position {position}, it must be 1 or 2");
		}

		[Fact]
		public void CannotAddTheSameCompetitorTwiceToTheSameMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);
			match.AddCompetitor(competitor, 1);

			Action act = () => match.AddCompetitor(competitor, 2);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Same competitor cannot be added twice to a match");
		}

		[Fact]
		public void CanAddByeAsBothCompetitorsToAMatch()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(CompetitorId.Bye, 1);
			match.AddCompetitor(CompetitorId.Bye, 2);

			match.Competitors[0].Should().Be(CompetitorId.Bye);
			match.Competitors[1].Should().Be(CompetitorId.Bye);
		}

		[Fact]
		public void CanScheduleAMatchIfItHasTwoCompetitors()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.State.Should().Be(MatchState.Scheduled);
			match.Court.Should().Be("Centre Court");
		}

		[Fact]
		public void CannotScheduleAMatchIfItDoesNotHaveTwoCompetitors()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor, 2);

			Action act = () => match.Schedule("Centre Court");

			act.Should()
				.Throw<Exception>()
				.WithMessage("The match does not have 2 competitors");
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void MustSpecifyACourtWhenSchedulingAMatch(string court)
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			Action act = () => match.Schedule(court);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Value can not be null or empty string (Parameter 'court')");
		}

		[Fact]
		public void CanScheduleAMatchThatHasAlreadyBeenScheduled()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.Schedule("Court One");

			match.State.Should().Be(MatchState.Scheduled);
			match.Court.Should().Be("Court One");
		}

		[Fact]
		public void CanUnscheduleAMatchThatHasBeenScheduled()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.Unschedule();

			match.State.Should().Be(MatchState.Created);
			match.Court.Should().BeNull();
		}

		[Fact]
		public void CannotUnscheduleAMatchThatHasNotBeenScheduled()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			Action act = () => match.Unschedule();

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Unschedule not allowed for a tournament in the state Created");
		}

		[Fact]
		public void CanAddMatchResultToAMatchWhereCompetitor1WasTheWinner()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, null);

			match.Winner.Should().Be(competitor1);
			match.State.Should().Be(MatchState.Completed);
		}

		[Fact]
		public void CanAddMatchResultToAMatchWhereCompetitor2WasTheWinner()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, null);

			match.Winner.Should().Be(competitor2);
			match.State.Should().Be(MatchState.Completed);
		}

		[Fact]
		public void CannotAddMatchResultToAMatchThatHasNotBeenScheduled()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, null);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Record Match Result not allowed for a tournament in the state Created");
		}

		[Fact]
		public void CannotAddMatchResultToAMatchThatAlreadyHasAResult()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);


			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, null);

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, null);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Record Match Result not allowed for a tournament in the state Completed");
		}

		[Fact]
		public void CannotAddCompetitorsToAMatchAlreadyScheduled()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);


			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			Action act = () => match.AddCompetitor(competitor1, 1);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Add Competitor not allowed for a tournament in the state Scheduled");
		}

		[Fact]
		public void CannotAddCompetitorsToAMatchAlreadyCompleted()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, null);

			Action act = () => match.AddCompetitor(competitor1, 1);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Add Competitor not allowed for a tournament in the state Completed");
		}

		[Fact]
		public void CannotScheduleToAMatchAlreadyCompleted()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, null);

			Action act = () => match.Schedule("Court One");

			act.Should()
				.Throw<Exception>()
				.WithMessage("Cannot schedule a match that has already been completed.");
		}

		[Fact]
		public void CannotUnscheduleToAMatchAlreadyCompleted()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Schedule("Centre Court");

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, null);

			Action act = () => match.Unschedule();

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action unschedule not allowed for a tournament in the state Completed");
		}
	}
}
