using Cqrs.Common.Data;
using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using Players.Contract;

#nullable disable

namespace Players.Query;

public sealed class GetPlayerSummaryList : IQuery<IList<PlayerSummaryDto>>
{
}

public sealed class GetPlayerSummaryListHandler
	: IQueryHandler<GetPlayerSummaryList, IList<PlayerSummaryDto>>
{
	private readonly ConnectionString _connectionString;

	public GetPlayerSummaryListHandler(ConnectionString connectionString)
	{
		_connectionString = connectionString;
	}

	public IList<PlayerSummaryDto> Handle(GetPlayerSummaryList query)
	{
		var sql = GetSqlQuery();

		using SqlConnection connection = new(_connectionString.Value);

		return connection.Query<PlayerSummaryDto>(sql)
			.ToList();
	}

	private static string GetSqlQuery()
	{
		return @"SELECT p.Id, p.FirstName + ' ' + p.LastName AS FullName,
					p.Gender, p.Plays, p.Height, p.SinglesRank, p.DoublesRank
				from Player p";
	}
}
