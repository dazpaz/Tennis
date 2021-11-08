using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentManagement.Domain
{
	public class Competitor : Entity<CompetitorId>
	{
		public TournamentId TournamentId { get; private set; }
		public EventType EventType { get; private set; }
		public Guid EventEntryId { get; private set; }
		public int Seeding { get; private set; }
		public ReadOnlyCollection<string> PlayerNames { get; private set; }

		private IList<string> _playersNames { get; set; }

		private Competitor(CompetitorId id, ICollection<string> playersNames) : base(id)
		{
			_playersNames = new List<string>(playersNames);
			PlayerNames = new ReadOnlyCollection<string>(_playersNames);
		}

		public static Competitor Create(TournamentId tournamentId, EventType eventType,
			Guid eventEntryId, int seeding, ICollection<string> playersNames)
		{
			Guard.ForIntegerOutOfRange(seeding, 0, 32, "seeding");
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
				throw new Exception("Wrong number of players for this event type");
			}
		}
	}
}
