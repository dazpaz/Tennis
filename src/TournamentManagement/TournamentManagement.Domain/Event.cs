using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class Event : Entity<Guid>
	{
		public EventType EventType { get; private set; }
		public bool IsSinglesEvent { get; private set; }
		public MatchFormat MatchFormat { get; private set; }
		public EventSize EventSize { get; private set; }


		private Event(Guid id) : base(id)
		{
		}

		public static Event Create(EventType eventType, int entrantsLimit, int numberOfSeeds, MatchFormat matchFormat)
		{
			var tennisEvent = new Event(Guid.NewGuid());
			tennisEvent.SetAttributeDetails(eventType, entrantsLimit, numberOfSeeds, matchFormat);
			return tennisEvent;
		}

		public void UpdateDetails(EventType eventType, int entrantsLimit, int numberOfSeeds, MatchFormat matchFormat)
		{
			SetAttributeDetails(eventType, entrantsLimit, numberOfSeeds, matchFormat);
		}

		private void SetAttributeDetails(EventType eventType, int entrantsLimit,
			int numberOfSeeds, MatchFormat matchFormat)
		{
			EventType = eventType;
			MatchFormat = matchFormat;
			IsSinglesEvent = (eventType == EventType.MensSingles || eventType == EventType.WomensSingles);
			EventSize = new EventSize(entrantsLimit, numberOfSeeds);
		}
	}
}
