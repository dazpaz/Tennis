using Ardalis.GuardClauses;
using DomainDesign.Common;
using System.Collections.Generic;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate.Guards;

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
			Guard.Against.NotASinglesEventType(eventType);
			Guard.Against.WrongGenderForEventType(eventType, new List<Player> { player });

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
			Guard.Against.NotADoublesEventType(eventType);
			Guard.Against.WrongGenderForEventType(eventType, new List<Player> { playerOne, playerTwo });

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
	}
}
