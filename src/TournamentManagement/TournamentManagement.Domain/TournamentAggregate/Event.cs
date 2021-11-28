using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Event : Entity<EventId>
	{
		public EventType EventType { get; private set; }
		public bool SinglesEvent { get; private set; }
		public MatchFormat MatchFormat { get; private set; }
		public EventSize EventSize { get; private set; }
		public bool IsCompleted { get; private set; }

		private Event(EventId id) : base(id)
		{
		}

		public static Event Create(EventType eventType, int entrantsLimit, int numberOfSeeds, MatchFormat matchFormat)
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
