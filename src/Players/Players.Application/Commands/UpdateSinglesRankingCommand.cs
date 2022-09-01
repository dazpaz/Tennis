using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands;

public class UpdateSinglesRankingCommand : UpdateRankingBase
{
	private UpdateSinglesRankingCommand(PlayerId playerId, Ranking ranking,
		RankingPoints rankingPoints, DateTime date)
		: base(playerId, ranking, rankingPoints, date)
	{
	}

	public static Result<ICommand> Create(Guid id, int rank, int points, DateTime date)
	{
		PlayerId playerId;
		try
		{
			playerId = new PlayerId(id);
		}
		catch (Exception ex)
		{
			return Result.Failure<ICommand>(ex.Message);
		}

		var ranking = Ranking.Create(rank);
		if (ranking.IsFailure) return Result.Failure<ICommand>(ranking.Error);

		var rankingPoints = RankingPoints.Create(points);
		if (rankingPoints.IsFailure) return Result.Failure<ICommand>(rankingPoints.Error);

		return new UpdateSinglesRankingCommand(playerId, ranking.Value, rankingPoints.Value, date);
	}
}
