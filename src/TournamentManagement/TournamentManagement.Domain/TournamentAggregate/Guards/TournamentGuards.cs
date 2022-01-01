using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Domain.TournamentAggregate;

namespace Ardalis.GuardClauses
{
	public static class TournamentGuards
	{
		public static void TournamentActionInWrongState(this IGuardClause guardClause, TournamentState expectedState,
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

		public static void NoEvents<T>(this IGuardClause guardClause, ICollection<T> events)
		{
			Guard.Against.CollectionIsEmpty(events,
				"Tournament must have at least one event to open it for entries");
		}

		public static void DuplicateEventType(this IGuardClause guardClause, IEnumerable<Event> events, EventType eventType)
		{
			if (events.Any(e => e.EventType == eventType))
			{
				throw new Exception($"Tournament already has an event of type {eventType}");
			}
		}

		public static void MissingEventType(this IGuardClause guardClause, IEnumerable<Event> events, EventType eventType)
		{
			if (!events.Any(e => e.EventType == eventType))
			{
				throw new Exception($"Tournament does not have an event of type {eventType}");
			}
		}
	}
}