using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Data;
using TournamentManagement.Data.Repository;
using TournamentManagement.Domain;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.RoundAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Console
{
	class Program
	{
		private static string _connectionString;
		private static bool _useConsoleLogger;

		public static void Main()
		{
			_connectionString = GetConnectionString();
			_useConsoleLogger = true;  // IHostingEnvironment.IsDevelopment();

			EnsureDatabaseIsCreated();

			var playerGuid = CreatePlayers();
			ReadPlayer(playerGuid);

			var venueId = CreateVenue();
			TestDuplicateCourtNames(venueId);
			AddAnExtraCourt(venueId);

			var tournamentId = CreateTournament(venueId);
			ReadTournament(tournamentId);
			ReadTournamentAndEvents(tournamentId);
			ReadTournamentAndVenue(tournamentId);
			ReadEntries(tournamentId);

			var competitorId = CreateCompetitor(tournamentId);
			ReadCompetitor(competitorId);

			var roundId = CreateRound(tournamentId);
			ReadRound(roundId);
		}

		private static void EnsureDatabaseIsCreated()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			context.Database.EnsureCreated();
		}

		private static Guid CreatePlayers()
		{
			var dorisGuid = Guid.NewGuid();
			var steveGuid = Guid.NewGuid();

			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);

			var player1 = Player.Register(new PlayerId(steveGuid), "Steve Serve", 32, 123, Gender.Male);
			var player2 = Player.Register(new PlayerId(dorisGuid), "Doris Dropshot", 4, 56, Gender.Female);

			context.Players.Add(player1);
			context.Players.Add(player2);

			context.SaveChanges();

			return dorisGuid;
		}

		private static void ReadPlayer(Guid playerGuid)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var player = context.Players.Find(new PlayerId(playerGuid));
		}

		private static IVenueRepository GetVenueRepository()
		{
			var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new VenueRepository(context);
			return repository;
		}

		private static VenueId CreateVenue()
		{
			using var repository = GetVenueRepository();

			var venueId = new VenueId(Guid.NewGuid());
			var wimbledon = Venue.Create(venueId, "Wimbledon", Surface.Grass);
			wimbledon.AddCourt(new CourtId(), "Centre Court", 14979);
			wimbledon.AddCourt(new CourtId(), "Court 1", 12345);

			var queens = Venue.Create(new VenueId(), "Queens Club", Surface.Grass);

			repository.Add(wimbledon);
			repository.Add(queens);

			repository.SaveChanges();

			return venueId;
		}

		private static void TestDuplicateCourtNames(VenueId venueId)
		{
			using var repository = GetVenueRepository();

			var venue = repository.GetById(venueId);

			try
			{
				venue.AddCourt(new CourtId(), "Centre Court", 14979);
				repository.SaveChanges();
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static void AddAnExtraCourt(VenueId venueId)
		{
			using var repository = GetVenueRepository();

			var venue = repository.GetById(venueId);
			venue.AddCourt(new CourtId(), "New Court", 100);
			repository.SaveChanges();
		}

		private static TournamentId CreateTournament(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);

			var venue = context.Venues.Find(venueId); // Or user the venue repository - Note: in this case we dont want to load the whole thing

			var tournament = Tournament.Create("Wimbledon 2022", TournamentLevel.GrandSlam,
				new DateTime(2022, 07, 22), new DateTime(2022, 07, 29), venue);

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32, new MatchFormat(5, SetType.TieBreakAtTwelveAll)));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 128, 32, new MatchFormat(3, SetType.TieBreakAtTwelveAll)));

			tournament.OpenForEntries();

			var player = Player.Register(new PlayerId(), "Edward Entered", 12, 123, Gender.Male);
			tournament.EnterEvent(EventType.MensSingles, player);

			context.Tournaments.Add(tournament);
			context.SaveChanges();

			return tournament.Id;
		}

		private static void ReadTournament(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments.Find(tournamentId);
		}

		private static void ReadTournamentAndEvents(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments
				.Include(t => t.Events)
				.First(t => t.Id == tournamentId);
		}

		private static void ReadTournamentAndVenue(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments
				.Include(t => t.Venue)
				.First(t => t.Id == tournamentId);
		}

		private static void ReadEntries(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments.Find(tournamentId);

			var tennisEvent = tournament.Events.FirstOrDefault(e => e.EventType == EventType.MensSingles);
			var name = tennisEvent.Entries[0].PlayerOne.Name;
		}

		private static CompetitorId CreateCompetitor(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments.Find(tournamentId);
			var tennisEvent = tournament.Events.FirstOrDefault(e => e.EventType == EventType.MensSingles);
			var player = tennisEvent.Entries[0].PlayerOne;
			var competitor = Competitor.Create(tournament, EventType.MensSingles, new Seeding(1), player.Name);

			context.Competitors.Add(competitor);
			context.SaveChanges();

			return competitor.Id;
		}

		private static void ReadCompetitor(CompetitorId competitorId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var competitor = context.Competitors.Find(competitorId);
		}

		private static RoundId CreateRound(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournament = context.Tournaments.Find(tournamentId);

			var round = Round.Create(tournament, EventType.MensSingles, 1, 32);

			context.Rounds.Add(round);
			context.SaveChanges();

			return round.Id;
		}

		private static void ReadRound(RoundId roundId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var round = context.Rounds.Find(roundId);
			var title = round.Tournament.Title;
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
