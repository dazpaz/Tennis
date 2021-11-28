using FluentAssertions;
using System;
using System.Collections.Generic;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.MatchAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.MatchAggregate
{
	public class MatchTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateMatchAndItIsCreatedCorrectly()
		{
			var matchSlot = new MatchSlot(new MatchId(), 2);
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, matchSlot);

			match.Id.Id.Should().NotBe(Guid.Empty);
			match.Format.Should().Be(MatchFormat.OneSetMatchWithTwoGamesClear);
			match.State.Should().Be(MatchState.Created);
			match.Outcome.Should().Be(MatchOutcome.AwaitingOutcome);
			match.WinnersNextMatch.Should().Be(matchSlot);

			match.Competitors.Count.Should().Be(2);
			match.Competitors[0].Should().BeNull();
			match.Competitors[1].Should().BeNull();

			match.MatchScore.Should().BeNull();

			match.Court.Should().BeNull();
			match.MatchWinner.Should().BeNull();
		}

		[Fact]
		public void CanAddACompetitorInSlotOneOfTheMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			match.AddCompetitor(competitor, 1);

			match.Competitors[0].Should().Be(competitor);
			match.Competitors[1].Should().BeNull();
		}

		[Fact]
		public void CanAddACompetitorInSlotTwoOfTheMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			match.AddCompetitor(competitor, 2);

			match.Competitors[0].Should().BeNull();
			match.Competitors[1].Should().Be(competitor);
			
		}

		[Fact]
		public void CanAddBothCompetitorsToTheMatch()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			match.Competitors[0].Should().Be(competitor1);
			match.Competitors[1].Should().Be(competitor2);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		public void CannotAddACompetitorIntoAnInvalidSlot(int slot)
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			Action act = () => match.AddCompetitor(competitor, slot);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Invalid Competitor Slot {slot}, it must be 1 or 2");
		}

		[Fact]
		public void CannotAddTheSameCompetitorTwiceToTheSameMatch()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);
			match.AddCompetitor(competitor, 1);

			Action act = () => match.AddCompetitor(competitor, 2);

			act.Should()
				.Throw<Exception>()
				.WithMessage($"Same competitor cannot be added twice to a match");
		}

		[Fact]
		public void CanAddByeAsBothCompetitorsToAMatch()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			match.AddCompetitor(CompetitorId.Bye, 1);
			match.AddCompetitor(CompetitorId.Bye, 2);

			match.Competitors[0].Should().Be(CompetitorId.Bye);
			match.Competitors[1].Should().Be(CompetitorId.Bye);
		}

		[Fact]
		public void CanScheduleAMatchOnACourtIfItHasTwoCompetitors()
		{
			var match = CreateMatchWithTwoCompetitors();

			match.Schedule("Centre Court");

			match.State.Should().Be(MatchState.Scheduled);
			match.Court.Should().Be("Centre Court");
		}

		[Fact]
		public void CannotScheduleAMatchIfItDoesNotHaveTwoCompetitors()
		{
			var competitor = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

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
			var match = CreateMatchWithTwoCompetitors();

			Action act = () => match.Schedule(court);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Value can not be null or empty string (Parameter 'court')");
		}

		[Fact]
		public void CanScheduleAMatchThatHasAlreadyBeenScheduled()
		{
			var match = CreateScheduledMatch();

			match.Schedule("Court One");

			match.State.Should().Be(MatchState.Scheduled);
			match.Court.Should().Be("Court One");
		}

		[Fact]
		public void CanUnscheduleAMatchThatHasBeenScheduled()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

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
			var match = CreateMatchWithTwoCompetitors();

			Action act = () => match.Unschedule();

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Unschedule not allowed for a tournament in the state Created");
		}


		[Fact]
		public void CannotAddMatchResultWhereTheOutcomeIsAwaitingOutcome()
		{
			var match = CreateScheduledMatch();

			Action act = () => match.RecordMatchResult(MatchOutcome.AwaitingOutcome, Winner.Unknown);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Cannot record match result with an outcome of 'awaiting outcome'");
		}

		[Fact]
		public void CanAddMatchResultWhenTheOutcomeWasABye()
		{
			var match = CreateScheduledMatch();

			match.RecordMatchResult(MatchOutcome.Bye, Winner.Competitor2);

			match.Outcome.Should().Be(MatchOutcome.Bye);
			match.State.Should().Be(MatchState.Completed);
			match.MatchWinner.Should().Be(match.Competitors[1]);
		}

		[Fact]
		public void CanAddMatchResultWhenTheOutcomeWasAWalkover()
		{
			var match = CreateScheduledMatch();

			match.RecordMatchResult(MatchOutcome.Walkover, Winner.Competitor1);

			match.Outcome.Should().Be(MatchOutcome.Walkover);
			match.State.Should().Be(MatchState.Completed);
			match.MatchWinner.Should().Be(match.Competitors[0]);
		}

		[Theory]
		[InlineData(MatchOutcome.Bye)]
		[InlineData(MatchOutcome.Walkover)]
		[InlineData(MatchOutcome.Retired)]
		public void WhenOutcomeIsByeOrWalkoverOrRetiredTheWinnerCannotBeUnknown(MatchOutcome outcome)
		{
			var match = CreateScheduledMatch();

			Action act = () => match.RecordMatchResult(outcome, Winner.Unknown);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Winner cannot be Unkown");
		}

		[Fact]
		public void CanAddMatchResultSpecifyingTheWinnerWhenTheOutcomeWasRetired()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(4, 0)
			};

			match.RecordMatchResult(MatchOutcome.Retired, Winner.Competitor2, setScores);

			match.Outcome.Should().Be(MatchOutcome.Retired);
			match.MatchScore.Sets[0].Should().Be(1);
			match.MatchScore.Sets[1].Should().Be(0);
			match.State.Should().Be(MatchState.Completed);
			match.MatchWinner.Should().Be(match.Competitors[1]);
			match.MatchScore.Winner.Should().Be(Winner.Unknown);
		}

		[Fact]
		public void CanAddMatchResultToAMatchWhereCompetitor1WasTheWinner()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(7, 5)
			};

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, setScores);

			match.MatchWinner.Should().Be(match.Competitors[0]);
			match.MatchScore.Sets[0].Should().Be(2);
			match.MatchScore.Sets[1].Should().Be(0);
			match.MatchScore.Winner.Should().Be(Winner.Competitor1);
			match.State.Should().Be(MatchState.Completed);
		}

		[Fact]
		public void CanAddMatchResultToAMatchWhereCompetitor2WasTheWinner()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(5, 7),
				new SetScore(2, 6)
			};

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, setScores);

			match.MatchWinner.Should().Be(match.Competitors[1]);
			match.MatchScore.Sets[0].Should().Be(1);
			match.MatchScore.Sets[1].Should().Be(2);
			match.MatchScore.Winner.Should().Be(Winner.Competitor2);
			match.State.Should().Be(MatchState.Completed);
		}

		[Fact]
		public void CannotAddMatchResultWhereTheOutcomeWasCompletedIfTheSetScoresDontDefineAWinner()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(5, 7)
			};

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, setScores);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Winner cannot be Unkown");
		}

		[Fact]
		public void WhenAddingMatchResultWhereTheOutcomeWasCompletedTheSetScoresWhouldHaveTheCorrectWinner()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(7, 5)
			};

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, setScores);

			act.Should()
				.Throw<Exception>()
				.WithMessage("The winner and the set scores do not have the same winner");
		}

		[Fact]
		public void CannotAddMatchResultToAMatchThatHasNotBeenScheduled()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear, null);

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, null);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Record Match Result not allowed for a tournament in the state Created");
		}

		[Fact]
		public void CannotAddMatchResultToAMatchThatAlreadyHasAResult()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(5, 7),
				new SetScore(2, 6)
			};
			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, setScores);

			Action act = () => match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor2, setScores);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Record Match Result not allowed for a tournament in the state Completed");
		}

		[Fact]
		public void CannotAddCompetitorsToAMatchAlreadyScheduled()
		{
			var competitor = new CompetitorId();
			var match = CreateScheduledMatch();

			Action act = () => match.AddCompetitor(competitor, 1);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Add Competitor not allowed for a tournament in the state Scheduled");
		}

		[Fact]
		public void CannotAddCompetitorsToAMatchAlreadyCompleted()
		{
			var competitor = new CompetitorId();
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(6, 0)
			};

			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, setScores);

			Action act = () => match.AddCompetitor(competitor, 1);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action Add Competitor not allowed for a tournament in the state Completed");
		}

		[Fact]
		public void CannotScheduleAMatchAlreadyCompleted()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(6, 0)
			};
			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, setScores);

			Action act = () => match.Schedule("Court One");

			act.Should()
				.Throw<Exception>()
				.WithMessage("Cannot schedule a match that has already been completed.");
		}

		[Fact]
		public void CannotUnscheduleAMatchAlreadyCompleted()
		{
			var match = CreateScheduledMatch();
			var setScores = new List<SetScore>
			{
				new SetScore(6, 4),
				new SetScore(6, 0)
			};
			match.RecordMatchResult(MatchOutcome.Completed, Winner.Competitor1, setScores);

			Action act = () => match.Unschedule();

			act.Should()
				.Throw<Exception>()
				.WithMessage("Action unschedule not allowed for a tournament in the state Completed");
		}

		private static Match CreateScheduledMatch()
		{
			var match = CreateMatchWithTwoCompetitors();
			match.Schedule("Centre Court");
			return match;
		}

		private static Match CreateMatchWithTwoCompetitors()
		{
			var competitor1 = new CompetitorId();
			var competitor2 = new CompetitorId();
			var match = Match.Create(MatchFormat.ThreeSetMatchWithTwoGamesClear, null);

			match.AddCompetitor(competitor1, 1);
			match.AddCompetitor(competitor2, 2);

			return match;
		}
	}
}
