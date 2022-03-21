﻿using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Contract;

namespace TournamentManagement.Query
{
	public sealed class GetTournamentDetails : IQuery<TournamentDetailsDto>
	{
		public Guid TournamentId { get; }

		public GetTournamentDetails(Guid tournamentId)
		{
			TournamentId = tournamentId;
		}
	}

	public sealed class GetTournamentDetailsHandler
		: IQueryHandler<GetTournamentDetails, TournamentDetailsDto>
	{
		private readonly QueryConnectionString _connectionString;

		public GetTournamentDetailsHandler(QueryConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public TournamentDetailsDto Handle(GetTournamentDetails query)
		{
			using SqlConnection connection = new(_connectionString.Value);

			return ExecuteTournamentDetailsQuery(connection, query)
				.First();
		}

		private static IEnumerable<TournamentDetailsDto> ExecuteTournamentDetailsQuery(
			SqlConnection connection, GetTournamentDetails query)
		{
			var sql = GetSqlQuery();

			var tournamentDict = new Dictionary<Guid, TournamentDetailsDto>();

			var result = connection.Query<TournamentDetailsDto, EventDto, TournamentDetailsDto>(sql,
				(tournament, tennisEvent) =>
				{
					if (!tournamentDict.TryGetValue(tournament.Id, out var currentTournament))
					{
						currentTournament = tournament;
						tournamentDict.Add(currentTournament.Id, currentTournament);
					}
					if (tennisEvent != null)
					{
						currentTournament.Events.Add(tennisEvent);
						currentTournament.NumberOfEvents = currentTournament.Events.Count;
					}
					return currentTournament;
				},
				new { query.TournamentId });

			return result;
		}

		private static string GetSqlQuery()
		{
			return @"SELECT t.Id, t.Title, t.Level, t.StartDate, t.EndDate,
				 t.State, v.Id as VenueId, v.Name as VenueName,
				e.Id, e.EventType, e.NumberOfSets, e.FinalSetType,
				e.EntrantsLimit, e.IsCompleted, e.TournamentId, ee.NumberOfEntrants,
				CAST (CASE WHEN e.EventType < 2 THEN 1 ELSE 0 END AS BIT) AS IsSinglesEvent
			FROM dbo.Tournament t
			LEFT JOIN dbo.Venue v ON v.Id = t.VenueId
			LEFT JOIN dbo.Event e ON e.TournamentId = t.Id
			LEFT JOIN (SELECT ee.EventId, Count(*) NumberOfEntrants
				FROM dbo.EventEntry ee GROUP BY ee.EventId) ee
				ON ee.EventId = e.Id
			WHERE t.Id = @TournamentId";
		}
	}
}
