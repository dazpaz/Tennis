using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Contract;

namespace TournamentManagement.Query
{
	public sealed class GetTournamentSummaryList : IQuery<List<TournamentSummaryDto>>
	{
	}

	public sealed class GetTournamentSummaryListHandler
		: IQueryHandler<GetTournamentSummaryList, List<TournamentSummaryDto>>
	{
		private readonly string _connectionString;

		public GetTournamentSummaryListHandler(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<TournamentSummaryDto> Handle(GetTournamentSummaryList query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString);

			return connection
				.Query<TournamentSummaryDto>(sql)
				.ToList();
		}

		private static string GetSqlQuery()
		{
			return @"SELECT t.Id, t.Title, t.Level as TournamentLevel, t.StartDate, t.EndDate,
				t.State, v.Name as VenueName, e.NumberOfEvents
				FROM dbo.Tournament t
				LEFT JOIN dbo.Venue v ON v.Id = t.VenueId
				LEFT JOIN (SELECT e.TournamentId, Count(*) NumberOfEvents
					FROM dbo.Event e GROUP BY e.TournamentId) e
					ON e.TournamentId = t.Id";
		}
	}
}