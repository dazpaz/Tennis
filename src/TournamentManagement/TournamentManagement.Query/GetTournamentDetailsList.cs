using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Contract;

namespace TournamentManagement.Query
{
	public sealed class GetTournamentDetailsList : IQuery<List<TournamentDetailsDto>>
	{

	}

	public sealed class GetTournamentDetailsListHandler
		: IQueryHandler<GetTournamentDetailsList, List<TournamentDetailsDto>>
	{
		private readonly QueryConnectionString _connectionString;

		public GetTournamentDetailsListHandler(QueryConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public List<TournamentDetailsDto> Handle(GetTournamentDetailsList query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString.Value);

			var tournamentDict = new Dictionary<Guid, TournamentDetailsDto>();

			var result = connection.Query<TournamentDetailsDto, EventDto, TournamentDetailsDto>(sql,
				(tournament, tennisEvent) =>
				{
					if (!tournamentDict.TryGetValue(tournament.Id, out var currentTournament))
					{
						currentTournament = tournament;
						tournamentDict.Add(currentTournament.Id, currentTournament);
					}
					currentTournament.Events.Add(tennisEvent);
					return currentTournament;
				});

			return result.Distinct().ToList();
		}

		private static string GetSqlQuery()
		{
			return @"SELECT t.Id, t.Title, t.Level as TournamentLevel, t.StartDate,
				t.EndDate, t.State, v.Id as VenueId, v.Name as VenueName,
				e.Id, e.EventType, e.NumberOfSets, e.FinalSetType,
				e.EntrantsLimit, e.IsCompleted, e.TournamentId, ee.NumberOfEntrants
			FROM dbo.Tournament t
			LEFT JOIN dbo.Venue v ON v.Id = t.VenueId
			LEFT JOIN dbo.Event e ON e.TournamentId = t.Id
			LEFT JOIN (SELECT ee.EventId, Count(*) NumberOfEntrants
				FROM dbo.EventEntry ee GROUP BY ee.EventId) ee
				ON ee.EventId = e.Id";
		}
	}
}
