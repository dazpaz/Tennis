using Ardalis.GuardClauses;
using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate.Guards;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Event : Entity<EventId>
	{
		public EventType EventType { get; private set; }
		public bool SinglesEvent => IsSinglesEvent(EventType);
		public MatchFormat MatchFormat { get; private set; }
		public EventSize EventSize { get; private set; }
		public bool IsCompleted { get; private set; }

		private readonly List<EventEntry> _entries = new();
		public virtual IReadOnlyList<EventEntry> Entries => _entries.ToList();

		protected Event()
		{
		}

		private Event(EventId id) : base(id)
		{
		}

		internal static Event Create(EventType eventType, EventSize eventSize, MatchFormat matchFormat)
		{
			var tennisEvent = new Event(new EventId());
			tennisEvent.EventType = eventType;
			tennisEvent.SetAttributeDetails(eventSize, matchFormat);
			return tennisEvent;
		}

		public static bool IsSinglesEvent(EventType eventType)
		{
			return eventType == EventType.MensSingles || eventType == EventType.WomensSingles;
		}

		public void AmendDetails(EventSize eventSize, MatchFormat matchFormat)
		{
			Guard.Against.UpdatingCompletedEvent(IsCompleted);
			SetAttributeDetails(eventSize, matchFormat);
		}

		internal void CompleteEvent()
		{
			IsCompleted = true;
		}

		internal void EnterSinglesEvent(Player playerOne)
		{
			Guard.Against.NotASinglesEventType(EventType);
			Guard.Against.Null(playerOne, nameof(playerOne));
			Guard.Against.PlayerAlreadyEnteredInSingleEvent(Entries, playerOne);

			var entry = EventEntry.CreateSinglesEventEntry(EventType, playerOne);
			
			_entries.Add(entry);
		}

		internal void EnterDoublesEvent(Player playerOne, Player playerTwo)
		{
			Guard.Against.NotADoublesEventType(EventType);
			Guard.Against.Null(playerOne, nameof(playerOne));
			Guard.Against.Null(playerTwo, nameof(playerTwo));
			Guard.Against.PlayersAlreadyEnteredInDoublesEvent(Entries, playerOne, playerTwo);

			var entry = EventEntry.CreateDoublesEventEntry(EventType, playerOne, playerTwo);

			_entries.Add(entry);
		}

		internal void WithdrawFromSinglesEvent(Player playerOne)
		{
			Guard.Against.NotASinglesEventType(EventType);
			Guard.Against.Null(playerOne, nameof(playerOne));

			var entry = Guard.Against.PlayerNotEnteredInSingleEvent(Entries, playerOne);

			_entries.Remove(entry);
		}

		internal void WithdrawFromDoublesEvent(Player playerOne, Player playerTwo)
		{
			Guard.Against.NotADoublesEventType(EventType);
			Guard.Against.Null(playerOne, nameof(playerOne));
			Guard.Against.Null(playerTwo, nameof(playerTwo));
			var entry = Guard.Against.PlayersNotEnteredInDoublesEvent(Entries, playerOne, playerTwo);

			_entries.Remove(entry);
		}

		private void SetAttributeDetails(EventSize eventSize, MatchFormat matchFormat)
		{
			EventSize = eventSize;
			MatchFormat = matchFormat;
		}
	}
}
