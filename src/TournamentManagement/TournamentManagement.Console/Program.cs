using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
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
			ReadAndUpdatePlayer(playerGuid);

			var venueId = CreateVenue();
			TestDuplicateCourtNames(venueId);
			var courtId = AddAnExtraCourt(venueId);
			RemoveCourt(venueId, courtId);

			var tournamentId = CreateTournament(venueId);
			TestDuplicateEventTypes(tournamentId);
			ReadTournamentAndCloseEntries(tournamentId);
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
			var repository = new PlayerRepository(context);

			var player1 = Player.Register(new PlayerId(steveGuid), "Steve Serve", 32, 123, Gender.Male);
			var player2 = Player.Register(new PlayerId(dorisGuid), "Doris Dropshot", 4, 56, Gender.Female);

			repository.Add(player1);
			repository.Add(player2);

			context.SaveChanges();

			return dorisGuid;
		}

		private static void ReadAndUpdatePlayer(Guid playerGuid)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new PlayerRepository(context);

			var player = repository.GetById(new PlayerId(playerGuid));
			player.UpdateRankings(45, 67);
			context.SaveChanges();
		}

		private static VenueId CreateVenue()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new VenueRepository(context);

			var venueId = new VenueId(Guid.NewGuid());
			var wimbledon = Venue.Create(venueId, "Wimbledon", Surface.Grass);
			wimbledon.AddCourt(new CourtId(), "Centre Court", 14979);
			wimbledon.AddCourt(new CourtId(), "Court 1", 12345);

			var queens = Venue.Create(new VenueId(), "Queens Club", Surface.Grass);

			repository.Add(wimbledon);
			repository.Add(queens);

			context.SaveChanges();

			return venueId;
		}

		private static void TestDuplicateCourtNames(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new VenueRepository(context);

			var venue = repository.GetById(venueId);

			try
			{
				venue.AddCourt(new CourtId(), "Centre Court", 14979);
				context.SaveChanges();
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static CourtId AddAnExtraCourt(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new VenueRepository(context);

			var venue = repository.GetById(venueId);
			var courtId = new CourtId();
			venue.AddCourt(courtId, "New Court", 100);
			context.SaveChanges();

			return courtId;
		}

		private static void RemoveCourt(VenueId venueId, CourtId courtId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new VenueRepository(context);

			var venue = repository.GetById(venueId);
			venue.RemoveCourt(courtId);
			context.SaveChanges();
		}

		private static TournamentId CreateTournament(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournamentRepo = new TournamentRepository(context);
			var venueRepo = new VenueRepository(context);

			var venue = venueRepo.GetById(venueId);

			var tournament = Tournament.Create("Wimbledon 2022", TournamentLevel.GrandSlam,
				new DateTime(2022, 07, 22), new DateTime(2022, 07, 29), venue);

			tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
				new MatchFormat(5, SetType.TieBreakAtTwelveAll)));
			tournament.AddEvent(Event.Create(EventType.WomensSingles, 128, 32,
				new MatchFormat(3, SetType.TieBreakAtTwelveAll)));

			tournament.OpenForEntries();

			var player = Player.Register(new PlayerId(), "Edward Entered", 12, 123, Gender.Male);
			tournament.EnterEvent(EventType.MensSingles, player);

			tournamentRepo.Add(tournament);
			context.SaveChanges();

			return tournament.Id;
		}

		private static void TestDuplicateEventTypes(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new TournamentRepository(context);

			var tournament = repository.GetById(tournamentId);

			try
			{
				tournament.AddEvent(Event.Create(EventType.MensSingles, 128, 32,
					new MatchFormat(5, SetType.TieBreakAtTwelveAll)));
				context.SaveChanges();
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static void ReadTournamentAndCloseEntries(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new TournamentRepository(context); 
			
			var tournament = repository.GetById(tournamentId);
			tournament.CloseEntries();

			context.SaveChanges();
		}

		private static void ReadEntries(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var repository = new TournamentRepository(context);

			var tournament = repository.GetById(tournamentId);

			var tennisEvent = tournament.GetEvent(EventType.MensSingles);
			var name = tennisEvent.Entries[0].PlayerOne.Name;
		}

		private static CompetitorId CreateCompetitor(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournamentRepo = new TournamentRepository(context);
			var competitorRepo = new CompetitorRepository(context);

			var tournament = tournamentRepo.GetById(tournamentId);
			var tennisEvent = tournament.GetEvent(EventType.MensSingles);
			var player = tennisEvent.Entries[0].PlayerOne;
			var competitor = Competitor.Create(tournament, EventType.MensSingles, new Seeding(1), player.Name);

			competitorRepo.Add(competitor);
			context.SaveChanges();

			return competitor.Id;
		}

		private static void ReadCompetitor(CompetitorId competitorId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var competitorRepo = new CompetitorRepository(context);

			var competitor = competitorRepo.GetById(competitorId);
		}

		private static RoundId CreateRound(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var tournamentRepo = new TournamentRepository(context);
			var roundRepo = new RoundRepository(context);

			var tournament = tournamentRepo.GetById(tournamentId);

			var round = Round.Create(tournament, EventType.MensSingles, 1, 32);

			roundRepo.Add(round);
			context.SaveChanges();

			return round.Id;
		}

		private static void ReadRound(RoundId roundId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var roundRepo = new RoundRepository(context);
			var round = roundRepo.GetById(roundId);
			var tournamentTitle = round.Tournament.Title;
			var roundTitle = round.Title;
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
