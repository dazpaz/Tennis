using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Event : Entity<EventId>
	{
		public EventType EventType { get; private set; }
		public bool SinglesEvent => IsSinglesEvent(EventType);
		public MatchFormat MatchFormat { get; private set; }
		public EventSize EventSize { get; private set; }
		public bool IsCompleted { get; private set; }

		public virtual ReadOnlyCollection<EventEntry> Entries { get; private set; }

		private readonly IList<EventEntry> _entries;

		protected Event()
		{
		}

		private Event(EventId id) : base(id)
		{
			_entries = new List<EventEntry>();
			Entries = new ReadOnlyCollection<EventEntry>(_entries);
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
			GuardAgainstUpdatingCompletedEvent();
			SetAttributeDetails(eventType, entrantsLimit, numberOfSeeds, matchFormat);
		}

		public void MarkEventCompleted()
		{
			IsCompleted = true;
		}

		public EventEntryId Enter(Player playerOne, Player playerTwo = null)
		{
			EventEntry entry;

			if (SinglesEvent)
			{
				Guard.Against.Null(playerOne, nameof(playerOne));

				//ToDo: Move to a Guard
				var existingPlayersIds = _entries.Select(e => e.PlayerOne.Id);
				if (existingPlayersIds.Any(p => p == playerOne.Id))
				{
					throw new Exception($"Player {playerOne.Name} has already entered this event");
				}

				entry = EventEntry.CreateSinglesEventEntry(EventType, playerOne);
			}
			else
			{
				Guard.Against.Null(playerOne, nameof(playerOne));
				Guard.Against.Null(playerTwo, nameof(playerTwo));

				//ToDo: Move to a Guard
				var existingPlayersIds = _entries.Select(e => e.PlayerOne.Id)
					.Union(_entries.Select(e => e.PlayerTwo.Id));
				if (existingPlayersIds.Any(p => p == playerOne.Id))
				{
					throw new Exception($"Player {playerOne.Name} has already entered this event");
				}
				if (existingPlayersIds.Any(p => p == playerTwo.Id))
				{
					throw new Exception($"Player {playerTwo.Name} has already entered this event");
				}

				entry = EventEntry.CreateDoublesEventEntry(EventType, playerOne, playerTwo);
			}
			
			_entries.Add(entry);
			return entry.Id;
		}

		public void RemoveEntry(EventEntryId entryId)
		{
			var entry = _entries.FirstOrDefault(e => e.Id == entryId);
			if (entry != null) _entries.Remove(entry);
		}

		public void ClearEntries()
		{
			_entries.Clear();
		}

		private void SetAttributeDetails(EventType eventType, int entrantsLimit,
			int numberOfSeeds, MatchFormat matchFormat)
		{
			EventType = eventType;
			MatchFormat = matchFormat;
			EventSize = new EventSize(entrantsLimit, numberOfSeeds);
		}

		private void GuardAgainstUpdatingCompletedEvent()
		{
			if (IsCompleted)
			{
				throw new Exception("Cannot update the details of an event that is completed");
			}
		}
	}
}
