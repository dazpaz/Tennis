using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using TournamentManagement.Common;
using TournamentManagement.Data;
using TournamentManagement.Domain;
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
			ILoggerFactory loggerFactory = CreateLoggerFactory();

			var optionsBuilder = new DbContextOptionsBuilder<TournamentManagementDbContext>();
			optionsBuilder
				.UseSqlServer(connectionString)
				.UseLoggerFactory(loggerFactory)
				.EnableSensitiveDataLogging();

			EnsureDatabaseIsCreated(optionsBuilder);

			var playerGuid = CreatePlayers(optionsBuilder);
			ReadPlayer(optionsBuilder, playerGuid);

			var venueGuid = CreateVenue(optionsBuilder);
			ReadVenue(optionsBuilder, venueGuid);
			ReadVenueAndCourts(optionsBuilder, venueGuid);

			var tournamentGuid = CreateTournament(optionsBuilder, venueGuid);
			ReadTournament(optionsBuilder, tournamentGuid);
		}

		private static void EnsureDatabaseIsCreated(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);
			context.Database.EnsureCreated();
		}

		private static Guid CreatePlayers(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder)
		{
			var dorisGuid = Guid.NewGuid();
			var steveGuid = Guid.NewGuid();

			using var context = new TournamentManagementDbContext(optionsBuilder.Options);

			var player1 = Player.Register(new PlayerId(steveGuid), "Steve Serve", 32, 123, Gender.Male);
			var player2 = Player.Register(new PlayerId(dorisGuid), "Doris Dropshot", 4, 56, Gender.Female);

			context.Players.Add(player1);
			context.Players.Add(player2);

			context.SaveChanges();

			return dorisGuid;
		}

		private static void ReadPlayer(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder,
			Guid playerGuid)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);
			var player = context.Players.Find(new PlayerId(playerGuid));
		}

		private static Guid CreateVenue(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);

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

		private static void ReadVenue(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder, Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);
			var venue = context.Venues.Find(new VenueId(venueGuid));
		}

		private static void ReadVenueAndCourts(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder, Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);

			var venue = context.Venues
				.Include(v => v.Courts)
				.First(v => v.Id == new VenueId(venueGuid));
		}

		private static Guid CreateTournament(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder,
			Guid venueGuid)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);

			var tournament = Tournament.Create("Wimbledon 2022", TournamentLevel.GrandSlam,
				new DateTime(2022, 07, 22), new DateTime(2022, 07, 29), new VenueId(venueGuid));

			context.Tournaments.Add(tournament);
			context.SaveChanges();

			return tournament.Id.Id;
		}

		private static void ReadTournament(DbContextOptionsBuilder<TournamentManagementDbContext> optionsBuilder,
			Guid tournamentGuid)
		{
			using var context = new TournamentManagementDbContext(optionsBuilder.Options);
			var tournament = context.Tournaments.Find(new TournamentId(tournamentGuid));
		}

		private static ILoggerFactory CreateLoggerFactory()
		{
			return LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter((category, level) =>
						category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
					.AddConsole();
			});
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
