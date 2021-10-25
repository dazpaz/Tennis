using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class EventEntry : Entity<Guid>
	{
		public Guid TournamentId { get; private set; }
		public EventType EventType { get; private set; }
		public IReadOnlyCollection<Player> Players { get; private set; }
		public ushort Rank { get; private set; }

		private readonly IList<Player> _players;

		private EventEntry(Guid id) : base(id)
		{
			_players = new List<Player>();
			Players = new ReadOnlyCollection<Player>(_players);
		}

		public static EventEntry CreateSinglesEntry(Guid tournamentId, EventType eventType,
			Player player)
		{
			GuardIsSinglesEvent(eventType);

			var entry = CreateEntry(tournamentId, eventType);
			entry._players.Add(player);
			entry.Rank = player.SinglesRank;
			return entry;
		}

		public static EventEntry CreateDoublesEntry(Guid tournamentId, EventType eventType,
			Player playerOne, Player playerTwo)
		{
			GuardIsDoublesEvent(eventType);

			var entry = CreateEntry(tournamentId, eventType);
			entry._players.Add(playerOne);
			entry._players.Add(playerTwo);
			entry.Rank = entry._players.Min(p => p.DoublesRank);
			return entry;
		}

		private static void GuardIsSinglesEvent(EventType eventType)
		{
			if (!Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a singles event");
			}
		}

		private static void GuardIsDoublesEvent(EventType eventType)
		{
			if (Event.IsSinglesEvent(eventType))
			{
				throw new ArgumentException($"{eventType} is not a doubles event");
			}
		}

		private static EventEntry CreateEntry(Guid tournamentId, EventType eventType)
		{
			Guard.ForGuidIsNotEmpty(tournamentId, "tournamentId");

			var entry = new EventEntry(Guid.NewGuid())
			{
				TournamentId = tournamentId,
				EventType = eventType,
			};

			return entry;
		}
	}
}
