using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Contract;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Domain.TournamentAggregate.Guards
{
	public static class EventEntryGuards
	{
		public static void NotASinglesEventType(this IGuardClause guardClause, EventType eventType)
		{
			if (!Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a singles event");
			}
		}

		public static void NotADoublesEventType(this IGuardClause guardClause, EventType eventType)
		{
			if (Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a doubles event");
			}
		}

		public static void WrongGenderForEventType(this IGuardClause guardClause, EventType eventtype, IEnumerable<Player> players)
		{
			var maleCount = players.Count(p => p.Gender == Gender.Male);
			var femaleCount = players.Count(p => p.Gender == Gender.Female);

			if (!EventGenderValidator.IsValid(eventtype, maleCount, femaleCount))
			{
				throw new Exception($"Gender of players does not match the event type {eventtype}");
			}
		}
	}
}
