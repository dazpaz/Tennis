using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TournamentManagement.Domain.Common;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Event : Entity<EventId>
	{
		public TournamentId TournamentId { get; private set; }
		public EventType EventType { get; private set; }
		public bool SinglesEvent { get; private set; }
		public MatchFormat MatchFormat { get; private set; }
		public EventSize EventSize { get; private set; }
		public bool IsCompleted { get; private set; }

		public ReadOnlyCollection<EventEntry> Entries { get; private set; }

		private readonly IList<EventEntry> _entries;

		private Event(EventId id) : base(id)
		{
			_entries = new List<EventEntry>();
			Entries = new ReadOnlyCollection<EventEntry>(_entries);
		}

		public static Event Create(TournamentId tournamentId, EventType eventType, int entrantsLimit,
			int numberOfSeeds, MatchFormat matchFormat)
		{
			var tennisEvent = new Event(new EventId())
			{
				TournamentId = new TournamentId(tournamentId.Id)
			};
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

		public void AddEventEntry(EventEntry entry)
		{
			GuardAgainstEntryNotMatchingTheEvent(entry);
			// Guard Against same player entring the event more than once - right thing to do here?
			_entries.Add(entry);
		}

		private void GuardAgainstEntryNotMatchingTheEvent(EventEntry entry)
		{
			if (entry.EventType != EventType || entry.EventId != Id)
			{
				throw new Exception("Cannot add Entry to this Event as details do not match");
			}
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
			SinglesEvent = IsSinglesEvent(eventType);
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
