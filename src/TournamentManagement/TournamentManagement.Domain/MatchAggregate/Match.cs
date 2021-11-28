using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.CompetitorAggregate;

namespace TournamentManagement.Domain.MatchAggregate
{
	public class Match : Entity<MatchId>
	{
		public MatchFormat Format { get; private set; }
		public MatchState State { get; private set; }
		public MatchOutcome Outcome { get; private set; }
		public CompetitorId MatchWinner { get; private set; }
		public string Court { get; private set; }
		public MatchScore MatchScore { get; private set; }
		public MatchSlot WinnersNextMatch { get; private set; }
		public ReadOnlyCollection<CompetitorId> Competitors { get; }

		private readonly CompetitorId[] _competitors;

		private Match(MatchId id) : base(id)
		{
			_competitors = new CompetitorId[2];
			Competitors = new ReadOnlyCollection<CompetitorId>(_competitors);
		}

		public static Match Create(MatchFormat format, MatchSlot winnersNextMatch)
		{
			var match = new Match(new MatchId())
			{
				Format = format,
				State = MatchState.Created,
				Outcome = MatchOutcome.AwaitingOutcome,
				WinnersNextMatch = winnersNextMatch
			};

			return match;
		}

		public void AddCompetitor(CompetitorId competitorId, int slot)
		{
			GuardAgainstActionInInvalidState(MatchState.Created, "Add Competitor");
			GuardAgainstInvalidCompetitorSlot(slot);
			GuardAgainstDuplicateCompetitors(competitorId);

			_competitors[slot - 1] = competitorId;
		}

		public void Schedule(string court)
		{
			Guard.AgainstNullOrEmptyString(court, nameof(court));
			GuardAgainstMatchAlreadyCompleted();
			GuardAgainstMatchIsMissingCompetitors();

			Court = court;
			TransitionToState(MatchState.Scheduled);
		}

		public void Unschedule()
		{
			GuardAgainstActionInInvalidState(MatchState.Scheduled, "unschedule");

			Court = null;
			TransitionToState(MatchState.Created);
		}

		private void GuardAgainstMatchAlreadyCompleted()
		{
			if (State == MatchState.Completed)
			{
				throw new Exception("Cannot schedule a match that has already been completed.");
			}
		}

		public void RecordMatchResult(MatchOutcome outcome, Winner winner, ICollection<SetScore> setScores = null)
		{
			GuardAgainstActionInInvalidState(MatchState.Scheduled, "Record Match Result");
			GuardAgainstOutcomeIsAwaitingOutcome(outcome);

			if (outcome == MatchOutcome.Bye || outcome == MatchOutcome.Walkover)
			{
				GuardAgainstUnknownWinner(winner);
			}

			if (outcome == MatchOutcome.Retired)
			{
				GuardAgainstUnknownWinner(winner);
				MatchScore = new MatchScore(Format, setScores);
			}

			if (outcome == MatchOutcome.Completed)
			{
				MatchScore = new MatchScore(Format, setScores);
				GuardAgainstUnknownWinner(MatchScore.Winner);
				GuardAgainstMismatchOfWinners(winner, MatchScore.Winner);
			}

			Outcome = outcome;
			MatchWinner = _competitors[(int)winner];

			TransitionToState(MatchState.Completed);
		}

		private void TransitionToState(MatchState newState)
		{
			State = newState;
		}

		private static void GuardAgainstMismatchOfWinners(Winner winner1, Winner winner2)
		{
			if (winner1 != winner2)
			{
				throw new Exception("The winner and the set scores do not have the same winner");
			}
		}

		private static void GuardAgainstUnknownWinner(Winner winner)
		{
			if (winner == Winner.Unknown)
			{
				throw new Exception("Winner cannot be Unkown");
			}
		}

		private void GuardAgainstMatchIsMissingCompetitors()
		{
			if (Competitors.Count(c => c != null) != 2)
			{
				throw new Exception("The match does not have 2 competitors");
			}
		}

		private void GuardAgainstActionInInvalidState(MatchState validState, string action)
		{
			if (State != validState)
			{
				throw new Exception($"Action {action} not allowed for a tournament in the state {State}");
			}
		}

		private void GuardAgainstDuplicateCompetitors(CompetitorId competitorId)
		{
			if (competitorId == CompetitorId.Bye) return;

			if (Competitors.Any(c => c == competitorId))
			{
				throw new Exception("Same competitor cannot be added twice to a match");
			}
		}

		private static void GuardAgainstInvalidCompetitorSlot(int slot)
		{
			if (slot < 1 || slot > 2)
			{
				throw new Exception($"Invalid Competitor Slot {slot}, it must be 1 or 2");
			}
		}

		private static void GuardAgainstOutcomeIsAwaitingOutcome(MatchOutcome outcome)
		{
			if (outcome == MatchOutcome.AwaitingOutcome)
			{
				throw new Exception("Cannot record match result with an outcome of 'awaiting outcome'");
			}
		}
	}
}
