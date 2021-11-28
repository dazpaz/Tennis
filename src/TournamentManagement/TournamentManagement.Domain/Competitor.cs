using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Domain
{
	public class Competitor : Entity<CompetitorId>
	{
		public EventEntryId EventEntryId { get; private set; }
		public TournamentId TournamentId { get; private set; }
		public EventType EventType { get; private set; }
		public Seeding Seeding { get; private set; }
		public ReadOnlyCollection<string> PlayerNames { get; private set; }

		private IList<string> _playersNames { get; set; }

		private Competitor(CompetitorId id, ICollection<string> playersNames) : base(id)
		{
			_playersNames = new List<string>(playersNames);
			PlayerNames = new ReadOnlyCollection<string>(_playersNames);
		}

		public static Competitor Create(TournamentId tournamentId, EventType eventType,
			EventEntryId eventEntryId, Seeding seeding, ICollection<string> playersNames)
		{
			GuardAgainstWrongNumberOfPlayers(eventType, playersNames.Count);

			var competitor = new Competitor(new CompetitorId(), playersNames)
			{
				TournamentId = tournamentId,
				EventType = eventType,
				EventEntryId = eventEntryId,
				Seeding = seeding,
			};

			return competitor;
		}

		private static void GuardAgainstWrongNumberOfPlayers(EventType eventType, int playerCount)
		{
			var expectedPlayerCount = Event.IsSinglesEvent(eventType) ? 1 : 2;
			if (playerCount != expectedPlayerCount)
			{
				throw new Exception($"Competitor for {eventType} event must have {expectedPlayerCount} players");
			}
		}
	}
}
