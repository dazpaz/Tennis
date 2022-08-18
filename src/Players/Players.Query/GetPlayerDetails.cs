using Cqrs.Common.Data;
using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using Players.Contract;

#nullable disable

namespace Players.Query
{
	public sealed class GetPlayerDetails : IQuery<PlayerDetailsDto>
	{
		public Guid PlayerId { get; }

		public GetPlayerDetails(Guid playerId)
		{
			PlayerId = playerId;
		}
	}

	public sealed class GetPlayerDetailsHandler
		: IQueryHandler<GetPlayerDetails, PlayerDetailsDto>
	{
		private readonly ConnectionString _connectionString;

		public GetPlayerDetailsHandler(ConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public PlayerDetailsDto Handle(GetPlayerDetails query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString.Value);

			return connection.QuerySingle<PlayerDetailsDto>(sql, new 
			{
				query.PlayerId
			});
		}

		private static string GetSqlQuery()
		{
			return @"SELECT p.Id, p.FirstName, p.LastName, p.FirstName + ' ' + p.LastName AS FullName,
				p.Gender, p.DateOfBirth, p.Plays, p.Height, p.Country,
				p.SinglesRank, p.DoublesRank, p.SinglesRankingPoints,
				p.DoublesRankingPoints
			FROM dbo.Player p
			WHERE p.Id = @PlayerId";
		}
	}
}
