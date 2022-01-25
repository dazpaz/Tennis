using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Domain.TournamentAggregate.Guards
{
	public static class EventGuards
	{
		public static void UpdatingCompletedEvent(this IGuardClause guardClause, bool isCompleted)
		{
			if (isCompleted)
			{
				throw new Exception("Cannot update the details of an event that is completed");
			}
		}

		public static void PlayerAlreadyEnteredInSingleEvent(this IGuardClause guardClause,
			List<EventEntry> entries, Player player)
		{
			var existingPlayerIds = entries.Select(e => e.PlayerOne.Id);
			if (existingPlayerIds.Any(p => p == player.Id))
			{
				throw new Exception($"Player {player.Name} has already entered this event");
			}
		}

		public static void PlayersAlreadyEnteredInDoublesEvent(this IGuardClause guardClause,
			List<EventEntry> entries, Player playerOne, Player playerTwo)
		{
			var existingPlayerIds = entries.Select(e => e.PlayerOne.Id)
				.Union(entries.Select(e => e.PlayerTwo.Id));

			if (existingPlayerIds.Any(p => p == playerOne.Id))
			{
				throw new Exception($"Player {playerOne.Name} has already entered this event");
			}
			if (existingPlayerIds.Any(p => p == playerTwo.Id))
			{
				throw new Exception($"Player {playerTwo.Name} has already entered this event");
			}
		}

		public static EventEntry PlayerNotEnteredInSingleEvent(this IGuardClause guardClause,
			List<EventEntry> entries, Player playerOne)
		{
			var entry = entries.Find(e => e.PlayerOne == playerOne);
			if (entry == null)
			{
				throw new Exception("Player was not entered into the event");
			}
			return entry;
		}

		public static EventEntry PlayersNotEnteredInDoublesEvent(this IGuardClause guardClause,
			List<EventEntry> entries, Player playerOne, Player playerTwo)
		{
			var entry = entries.Find(e => e.PlayerOne == playerOne && e.PlayerTwo == playerTwo);
			if (entry == null)
			{
				throw new Exception("Players were not entered into the event");
			}
			return entry;
		}
	}
}
