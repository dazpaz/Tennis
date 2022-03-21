using CSharpFunctionalExtensions;
using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using System;
using TournamentManagement.Common;
using TournamentManagement.Contract;

namespace TournamentManagement.Query
{
	public sealed class GetEventDetails : IQuery<EventDto>
	{
		public Guid TournamentId { get; }
		public EventType EventType { get; }

		private GetEventDetails(Guid tournamentId, EventType eventType)
		{
			TournamentId = tournamentId;
			EventType = eventType;
		}

		public static Result<GetEventDetails> Create(Guid tournamentId, string eventType)
		{
			if (!Enum.TryParse(eventType, out EventType type))
			{
				return Result.Failure<GetEventDetails>("Invalid Event Type");
			}

			return new GetEventDetails(tournamentId, type);
		}
	}

	public sealed class GetEventDetailsHandler : IQueryHandler<GetEventDetails, EventDto>
	{
		private readonly QueryConnectionString _connectionString;

		public GetEventDetailsHandler(QueryConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public EventDto Handle(GetEventDetails query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString.Value);

			var tennisEvent = connection.QuerySingle<EventDto>(sql, new 
			{
				query.TournamentId,
				query.EventType
			});

			return tennisEvent;
		}

		private static string GetSqlQuery()
		{
			return @"SELECT e.Id, e.EventType,
						CAST (CASE WHEN e.EventType < 2 THEN 1 ELSE 0 END AS BIT) AS IsSinglesEvent,
						e.NumberOfSets, e.FinalSetType, e.EntrantsLimit, e.NumberOfSeeds,
						ee.NumberOfEntrants, e.IsCompleted, e.TournamentId
					FROM dbo.Event e
					LEFT JOIN (SELECT ee.EventId, Count(*) NumberOfEntrants
						FROM dbo.EventEntry ee GROUP BY ee.EventId) ee
						ON ee.EventId = e.Id
					WHERE e.TournamentId = @TournamentId AND e.EventType = @EventType";
		}
	}
}
