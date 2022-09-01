using DomainDesign.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands
{
	public abstract class UpdateRankingBase : ICommand
	{
		public PlayerId PlayerId { get; }
		public Ranking Ranking { get; }
		public RankingPoints RankingPoints { get; }
		public DateTime Date { get; }

		protected UpdateRankingBase(PlayerId playerId, Ranking ranking,
			RankingPoints rankingPoints, DateTime date)
		{
			PlayerId = playerId;
			Ranking = ranking;
			RankingPoints = rankingPoints;
			Date = date;
		}
	}
}
