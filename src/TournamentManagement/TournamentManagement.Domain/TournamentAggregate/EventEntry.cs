using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class EventEntry : Entity<EventEntryId>
	{
		public EventType EventType { get; private set; }
		public Player PlayerOne { get; private set; }
		public Player PlayerTwo { get; private set; }
		public ushort Rank { get; private set; }

		protected EventEntry()
		{
		}

		private EventEntry(EventEntryId id) : base(id)
		{
		}

		public static EventEntry CreateSinglesEventEntry(EventType eventType, Player player)
		{
			GuardAgainstDoublesEvent(eventType);
			GuardAgainstWrongGenderForEventType(eventType, new List<Player> { player });

			var entry = new EventEntry(new EventEntryId())
			{
				PlayerOne = player,
				EventType = eventType,
				Rank = player.SinglesRank
			};
			
			return entry;
		}

		public static EventEntry CreateDoublesEventEntry(EventType eventType,
			Player playerOne, Player playerTwo)
		{
			GuardAgainstSinglesEvent(eventType);
			GuardAgainstWrongGenderForEventType(eventType, new List<Player> { playerOne, playerTwo });

			var rank = playerOne.DoublesRank < playerTwo.DoublesRank
				? playerOne.DoublesRank
				: playerTwo.DoublesRank;

			var entry = new EventEntry(new EventEntryId())
			{
				PlayerOne = playerOne,
				PlayerTwo = playerTwo,
				EventType = eventType,
				Rank = rank
			};

			return entry;
		}

		private static void GuardAgainstSinglesEvent(EventType eventType)
		{
			if (Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a doubles event");
			}
		}

		private static void GuardAgainstDoublesEvent(EventType eventType)
		{
			if (!Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a singles event");
			}
		}

		private static void GuardAgainstWrongGenderForEventType(EventType eventtype, IEnumerable<Player> players)
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
