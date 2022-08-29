using CSharpFunctionalExtensions;
using Players.Query;

namespace Players.WebApi.Factory;

public class QueryFactory : IQueryFactory
{
	public GetPlayerDetails CreateGetPlayerDetailsQuery(Guid id)
	{
		return new GetPlayerDetails(id);
	}

	public GetPlayerSummaryList CreateGetPlayerSummaryListQuery()
	{
		return new GetPlayerSummaryList();
	}
}
