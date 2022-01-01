using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Data;
using TournamentManagement.Domain;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Console
{
	class Program
	{
		public static void Main()
		{
			string connectionString = GetConnectionString();
			bool useConsoleLogger = true;  // IHostingEnvironment.IsDevelopment();

			EnsureDatabaseIsCreated(connectionString, useConsoleLogger);

			var playerGuid = CreatePlayers(connectionString, useConsoleLogger);
			ReadPlayer(connectionString, useConsoleLogger, playerGuid);

			var venueGuid = CreateVenue(connectionString, useConsoleLogger);
			ReadVenue(connectionString, useConsoleLogger, venueGuid);
			ReadVenueAndCourts(connectionString, useConsoleLogger, venueGuid);

			var tournamentGuid = CreateTournament(connectionString, useConsoleLogger, venueGuid);
			ReadTournament(connectionString, useConsoleLogger, tournamentGuid);
			ReadTournamentAndEvents(connectionString, useConsoleLogger, tournamentGuid);
		}

		private static void EnsureDatabaseIsCreated(string connectionString, bool useConsoleLogger)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);
			context.Database.EnsureCreated();
		}

		private static Guid CreatePlayers(string connectionString, bool useConsoleLogger)
		{
			var dorisGuid = Guid.NewGuid();
			var steveGuid = Guid.NewGuid();

			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);

			var player1 = Player.Register(new PlayerId(steveGuid), "Steve Serve", 32, 123, Gender.Male);
			var player2 = Player.Register(new PlayerId(dorisGuid), "Doris Dropshot", 4, 56, Gender.Female);

			context.Players.Add(player1);
			context.Players.Add(player2);

			context.SaveChanges();

			return dorisGuid;
		}

		private static void ReadPlayer(string connectionString, bool useConsoleLogger, Guid playerGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);
			var player = context.Players.Find(new PlayerId(playerGuid));
		}

		private static Guid CreateVenue(string connectionString, bool useConsoleLogger)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);

			var venueGuid = Guid.NewGuid();
			var venueId = new VenueId(venueGuid);
			var wimbledon = Venue.Create(venueId, "Wimbledon", Surface.Grass);
			wimbledon.AddCourt(new CourtId(), "Centre Court", 14979);
			wimbledon.AddCourt(new CourtId(), "Court 1", 12345);

			var queens = Venue.Create(new VenueId(), "Queens Club", Surface.Grass);

			context.Venues.Add(wimbledon);
			context.Venues.Add(queens);

			context.SaveChanges();

			return venueGuid;
		}

		private static void ReadVenue(string connectionString, bool useConsoleLogger, Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);
			var venue = context.Venues.Find(new VenueId(venueGuid));
		}

		private static void ReadVenueAndCourts(string connectionString, bool useConsoleLogger, Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);

			var venue = context.Venues
				.Include(v => v.Courts)
				.First(v => v.Id == new VenueId(venueGuid));
		}

		private static Guid CreateTournament(string connectionString, bool useConsoleLogger, Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);

			var tournament = Tournament.Create("Wimbledon 2022", TournamentLevel.GrandSlam,
				new DateTime(2022, 07, 22), new DateTime(2022, 07, 29), new VenueId(venueGuid));

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32, new MatchFormat(5, SetType.TieBreakAtTwelveAll)));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 128, 32, new MatchFormat(3, SetType.TieBreakAtTwelveAll)));

			context.Tournaments.Add(tournament);
			context.SaveChanges();

			return tournament.Id.Id;
		}

		private static void ReadTournament(string connectionString, bool useConsoleLogger, Guid tournamentGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);
			var tournament = context.Tournaments.Find(new TournamentId(tournamentGuid));
		}

		private static void ReadTournamentAndEvents(string connectionString, bool useConsoleLogger, Guid tournamentGuid)
		{
			using var context = new TournamentManagementDbContext(connectionString, useConsoleLogger);
			var tournament = context.Tournaments
				.Include(t => t.Events)
				.First(t => t.Id == new TournamentId(tournamentGuid));
		}

		private static string GetConnectionString()
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			return configuration["ConnectionString"];
		}
	}
}
