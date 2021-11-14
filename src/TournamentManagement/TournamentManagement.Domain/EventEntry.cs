using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class EventEntry : Entity<EventEntryId>
	{
		public TournamentId TournamentId { get; private set; }
		public EventType EventType { get; private set; }
		public IReadOnlyCollection<Player> Players { get; private set; }
		public ushort Rank { get; private set; }

		private readonly IList<Player> _players;

		private EventEntry(EventEntryId id) : base(id)
		{
			_players = new List<Player>();
			Players = new ReadOnlyCollection<Player>(_players);
		}

		public static EventEntry CreateSinglesEventEntry(TournamentId tournamentId, EventType eventType,
			Player player)
		{
			GuardAgainstDoublesEvent(eventType);
			GuardAgainstWrongGenderForEventType(eventType, new List<Player> { player });

			var entry = CreateEntry(tournamentId, eventType);
			entry._players.Add(player);
			entry.Rank = player.SinglesRank;
			return entry;
		}

		public static EventEntry CreateDoublesEventEntry(TournamentId tournamentId, EventType eventType,
			Player playerOne, Player playerTwo)
		{
			GuardAgainstSinglesEvent(eventType);
			GuardAgainstWrongGenderForEventType(eventType, new List<Player> { playerOne, playerTwo });

			var entry = CreateEntry(tournamentId, eventType);
			entry._players.Add(playerOne);
			entry._players.Add(playerTwo);
			entry.Rank = entry._players.Min(p => p.DoublesRank);
			return entry;
		}

		private static EventEntry CreateEntry(TournamentId tournamentId, EventType eventType)
		{
			var entry = new EventEntry(new EventEntryId())
			{
				TournamentId = new TournamentId(tournamentId.Id),
				EventType = eventType,
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
