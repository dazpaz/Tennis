using System;
using TournamentManagement.Domain.TournamentAggregate;

namespace Ardalis.GuardClauses
{
	public static class TournamentAggregateGuards
	{
		public static void TornamentActionInWrongState(this IGuardClause guardClause, TournamentState expectedState, 
			TournamentState currentstate, string action)
		{
			if (currentstate != expectedState)
			{
				throw new Exception($"Action {action} not allowed for a tournament in the state {currentstate}");
			}
		}

		public static void DurationOutOfRange(this IGuardClause guardClause, TimeSpan timespan,
			int maxDurationInDays)
		{
			if (timespan.TotalDays < 0 || timespan.TotalDays >= maxDurationInDays)
			{
				throw new ArgumentException($"Duration must be 1 - {maxDurationInDays} days");
			}
		}
	}
}