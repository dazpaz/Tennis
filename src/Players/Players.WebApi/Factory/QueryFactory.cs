using Players.Query;

namespace Players.WebApi.Factory;

public class QueryFactory : IQueryFactory
{
	public GetCountryDetailsList CreateGetCountryDetailsListQuery()
	{
		return new GetCountryDetailsList();
	}

	public GetCountryDetails CreateGetCountryDetailsQuery(Guid id)
	{
		return new GetCountryDetails(id);
	}

	public GetPlayerDetails CreateGetPlayerDetailsQuery(Guid id)
	{
		return new GetPlayerDetails(id);
	}

	public GetPlayerSummaryList CreateGetPlayerSummaryListQuery()
	{
		return new GetPlayerSummaryList();
	}
}
