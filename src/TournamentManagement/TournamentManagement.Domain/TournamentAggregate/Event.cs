using Ardalis.GuardClauses;
using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
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

		public static Event Create(EventType eventType, int entrantsLimit,
			int numberOfSeeds, MatchFormat matchFormat)
		{
			var tennisEvent = new Event(new EventId());
			tennisEvent.SetAttributeDetails(eventType, entrantsLimit, numberOfSeeds, matchFormat);
			return tennisEvent;
		}

		public static bool IsSinglesEvent(EventType eventType)
		{
			return eventType == EventType.MensSingles || eventType == EventType.WomensSingles;
		}

		public void UpdateDetails(EventType eventType, int entrantsLimit, int numberOfSeeds, MatchFormat matchFormat)
		{
			Guard.Against.UpdatingCompletedEvent(IsCompleted);
			SetAttributeDetails(eventType, entrantsLimit, numberOfSeeds, matchFormat);
		}

		public void CompleteEvent()
		{
			IsCompleted = true;
		}

		public void EnterEvent(Player playerOne, Player playerTwo = null)
		{
			EventEntry entry;

			if (SinglesEvent)
			{
				Guard.Against.Null(playerOne, nameof(playerOne));
				Guard.Against.PlayerAlreadyEnteredInSingleEvent(_entries, playerOne);

				entry = EventEntry.CreateSinglesEventEntry(EventType, playerOne);
			}
			else
			{
				Guard.Against.Null(playerOne, nameof(playerOne));
				Guard.Against.Null(playerTwo, nameof(playerTwo));
				Guard.Against.PlayersAlreadyEnteredInDoublesEvent(_entries, playerOne, playerTwo);

				entry = EventEntry.CreateDoublesEventEntry(EventType, playerOne, playerTwo);
			}
			
			_entries.Add(entry);
		}

		public void WithdrawFromEvent(Player playerOne, Player playerTwo = null)
		{
			
			if (SinglesEvent)
			{
				Guard.Against.Null(playerOne, nameof(playerOne));
				var entry = _entries.Find(e => e.PlayerOne == playerOne);
				Guard.Against.Null(entry, nameof(playerOne), "Player was not entered into the event");

				_entries.Remove(entry);
			}
			else
			{
				Guard.Against.Null(playerOne, nameof(playerOne));
				Guard.Against.Null(playerTwo, nameof(playerTwo));
				var entry = _entries.Find(e => e.PlayerOne == playerOne && e.PlayerTwo == playerTwo);
				Guard.Against.Null(entry, nameof(playerOne), "Players were not entered into the event");

				_entries.Remove(entry);
			}
		}

		private void SetAttributeDetails(EventType eventType, int entrantsLimit,
			int numberOfSeeds, MatchFormat matchFormat)
		{
			EventType = eventType;
			MatchFormat = matchFormat;
			EventSize = new EventSize(entrantsLimit, numberOfSeeds);
		}
	}
}
