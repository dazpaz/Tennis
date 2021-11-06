using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class Match : Entity<MatchId>
	{
		public MatchFormat Format { get; private set; }
		public MatchState State { get; private set; }
		public MatchOutcome Outcome { get; private set; }
		public CompetitorId Winner { get; private set; }
		public string Court { get; private set; }

		public ReadOnlyCollection<SetScore> SetScores { get; }
		public ReadOnlyCollection<CompetitorId> Competitors { get; }

		private List<SetScore> _setScores;
		private CompetitorId[] _competitors;

		private Match(MatchId id) : base(id)
		{
			_setScores = new List<SetScore>();
			SetScores = new ReadOnlyCollection<SetScore>(_setScores);

			_competitors = new CompetitorId[2];
			Competitors = new ReadOnlyCollection<CompetitorId>(_competitors);
		}

		public static Match Create(MatchFormat format)
		{
			var match = new Match(new MatchId())
			{
				Format = format,
				State = MatchState.Created,
				Outcome = MatchOutcome.AwaitingOutcome
			};

			return match;
		}

		public void AddCompetitor(CompetitorId competitorId, int position)
		{
			GuardAgainstActionInInvalidState(MatchState.Created, "Add Competitor");
			GuardAgainstInvalidCompetitorPosition(position);
			GuardAgainstDuplicateCompetitors(competitorId);

			_competitors[position - 1] = competitorId;
		}

		public void Schedule(string court)
		{
			Guard.ForNullOrEmptyString(court, "court");
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

		public void RecordMatchResult(MatchOutcome outcome, Winner winner, ICollection<SetScore> setScores)
		{
			GuardAgainstActionInInvalidState(MatchState.Scheduled, "Record Match Result");

			// if outcome == AwaitingOutcome --- throw exception
			// if outcome == completed --- make sure scores has correct number of sets and score inicate correct winner
			// if outcome == Retired --- should have at least 1 set score
			// if outcome == walkover --- setScores scores should be empty
			// if outcome == MatchOutcome.Bye --- setScores scores should be empty

			Outcome = outcome;
			Winner = _competitors[(int)winner];

			TransitionToState(MatchState.Completed);
		}

		private void TransitionToState(MatchState newState)
		{
			State = newState;
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

		private static void GuardAgainstInvalidCompetitorPosition(int position)
		{
			if (position < 1 || position > 2)
			{
				throw new Exception($"Invalid Competitor Position {position}, it must be 1 or 2");
			}
		}
	}
}
