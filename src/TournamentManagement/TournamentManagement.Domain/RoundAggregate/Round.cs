using Ardalis.GuardClauses;
using DomainDesign.Common;
using TournamentManagement.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Domain.RoundAggregate
{
	public class Round : AggregateRoot<RoundId>
	{
		private static readonly int[] AllowedCompetitorCount = { 128, 64, 32, 16, 8, 4 ,2 };

		public virtual Tournament Tournament { get; private set; }
		public EventType EventType { get; private set; }
		public int RoundNumber { get; private set; }
		public int CompetitorCount { get; private set; }
		public string Title => GetRoundTitle();

		protected Round()
		{
		}

		private Round(RoundId id) : base(id)
		{
		}
		
		public static Round Create(Tournament tournament, EventType eventType,
			int roundNumber, int competitorCount)
		{
			Guard.Against.Null(tournament, nameof(tournament));
			Guard.Against.IntegerOutOfRange(roundNumber, 1, 7, nameof(roundNumber));
			Guard.Against.ValueNotInSetOfAllowedValues(competitorCount, AllowedCompetitorCount, nameof(competitorCount));

			var round = new Round(new RoundId())
			{
				Tournament = tournament,
				EventType = eventType,
				RoundNumber = roundNumber,
				CompetitorCount = competitorCount
			};

			return round;
		}

		private string GetRoundTitle()
		{
			if (CompetitorCount == 2) return "Final";
			if (CompetitorCount == 4) return "Semi-Final";
			if (CompetitorCount == 8) return "Quarter-Final";
			return $"Round of {CompetitorCount}";
		}
	}
}
