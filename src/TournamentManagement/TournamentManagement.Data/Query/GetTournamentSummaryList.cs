using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Data.Query
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

			List<DbQueryModel> tournaments = connection
				.Query<DbQueryModel>(sql)
				.ToList();

			return tournaments
				.Select(t => ConvertToDto(t))
				.ToList();
		}

		private static TournamentSummaryDto ConvertToDto(DbQueryModel dbModel)
		{
			return new TournamentSummaryDto()
			{
				Id = dbModel.Id,
				Title = dbModel.Title,
				TournamentLevel = ((TournamentLevel)dbModel.Level).ToString(),
				StartDate = dbModel.StartDate,
				EndDate = dbModel.EndDate,
				State = ((TournamentState)dbModel.State).ToString(),
				VenueName = dbModel.VenueName,
				NumberOfEvents = dbModel.NumberOfEvents
			};
		}

		private static string GetSqlQuery()
		{
			return @"SELECT t.Id, t.Title, t.Level as Level, t.StartDate, t.EndDate,
				t.State, v.Name as VenueName, e.NumberOfEvents
				FROM dbo.Tournament t
				LEFT JOIN dbo.Venue v ON v.Id = t.VenueId
				LEFT JOIN (SELECT e.TournamentId, Count(*) NumberOfEvents
					FROM dbo.Event e GROUP BY e.TournamentId) e
					ON e.TournamentId = t.Id";
		}

		private class DbQueryModel
		{
			public readonly Guid Id;
			public readonly string Title;
			public int Level;
			public DateTime StartDate;
			public DateTime EndDate;
			public int State;
			public string VenueName;
			public int NumberOfEvents;

			public DbQueryModel(Guid id, string title, int level, DateTime startDate,
				DateTime endDate, int state, string venueName, int numberOfEvents)
			{
				Id = id;
				Title = title;
				Level = level;
				StartDate = startDate;
				EndDate = endDate;
				State = state;
				VenueName = venueName;
				NumberOfEvents = numberOfEvents;
			}
		}
	}
}