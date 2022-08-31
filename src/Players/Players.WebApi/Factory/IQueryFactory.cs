using CSharpFunctionalExtensions;
using Players.Query;

namespace Players.WebApi.Factory;

public interface IQueryFactory
{
	GetPlayerDetails CreateGetPlayerDetailsQuery(Guid id);
	GetPlayerSummaryList CreateGetPlayerSummaryListQuery();
}
