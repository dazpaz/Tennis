using System;
using System.Collections.Generic;
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

		public static void DuplicateEventType<TKey, TValue>(this IGuardClause guardClause,
			IDictionary<EventType, Event> events, EventType eventType)
		{
			Guard.Against.DictionaryAlreadyContainsKey(events, eventType, 
				$"Tournament already has an event of type {eventType}");
		}

		public static void MissingEventType<TKey, TValue>(this IGuardClause guardClause,
			IDictionary<EventType, Event> events, EventType eventType)
		{
			Guard.Against.DictionaryDoesNotContainKey(events, eventType,
				$"Tournament does not have an event of type {eventType}");
		}
	}
}