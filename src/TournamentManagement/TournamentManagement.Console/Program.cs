using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TournamentManagement.Contract;
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
			CreateTestPlayers();
			CreateTestVenues();

			//var dorisId = CreatePlayers();
			//ReadAndUpdatePlayer(dorisId);

			//var venueId = CreateVenue();
			//TestDuplicateCourtNames(venueId);
			//var courtId = AddAnExtraCourt(venueId);
			//RemoveCourt(venueId, courtId);

			//var playerId = new PlayerId();
			//var tournamentId = CreateTournament(venueId, playerId);
			//TestDuplicateEventTypes(tournamentId);
			//TestAddingPlayersUsesLazyLoading(tournamentId, playerId);
			//AddTwoMorePlayersToEvents(tournamentId, dorisId);
			//ReadTournamentAndCloseEntries(tournamentId);
			//ReadEntries(tournamentId);

			//var competitorId = CreateCompetitor(tournamentId);
			//ReadCompetitor(competitorId);

			//var roundId = CreateRound(tournamentId);
			//ReadRound(roundId);
		}

		private static void EnsureDatabaseIsCreated()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			context.Database.EnsureCreated();
		}

		private static void CreateTestVenues()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var wimbledon = Venue.Create(new VenueId(), "Wimbledon", Surface.Grass);
			wimbledon.AddCourt(new CourtId(), "Center Court", 14979);
			wimbledon.AddCourt(new CourtId(), "No. 1 Court", 12345);

			uow.VenueRepository.Add(wimbledon);

			uow.VenueRepository.Add(Venue.Create(new VenueId(), "Roland Garros", Surface.Clay));
			uow.VenueRepository.Add(Venue.Create(new VenueId(), "Flushing Meadows", Surface.Hard));
			uow.VenueRepository.Add(Venue.Create(new VenueId(), "Melbourne Park", Surface.Hard));

			uow.SaveChanges();
		}

		private static void CreateTestPlayers()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Novak Djokovic", 1, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Daniil Medvedev", 2, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Alexander Zverev", 3, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Stefanos Tsitsipas", 4, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Rafael Nadal", 5, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Matteo Berrettini", 6, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Andrey Rublev", 7, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Casper Ruud", 8, 101, Gender.Male));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Felix Auger-Aliassime", 9, 101, Gender.Male));

			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Ashleigh Barty", 1, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Aryna Sabalenka", 2, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Barbora Krejcikova", 3, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Karolina Pliskova", 4, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Paula Badosa", 5, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Anett Kontaveit", 6, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Garbiñe Muguruza", 7, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Maria Sakkari", 8, 101, Gender.Female));
			uow.PlayerRepository.Add(Player.Register(new PlayerId(), "Iga Swiatek", 9, 101, Gender.Female));

			uow.SaveChanges();
		}

		private static PlayerId CreatePlayers()
		{
			var steveId = new PlayerId();
			var dorisId = new PlayerId();

			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var player1 = Player.Register(steveId, "Steve Serve", 32, 123, Gender.Male);
			var player2 = Player.Register(dorisId, "Doris Dropshot", 4, 56, Gender.Female);

			uow.PlayerRepository.Add(player1);
			uow.PlayerRepository.Add(player2);

			uow.SaveChanges();

			return dorisId;
		}

		private static void ReadAndUpdatePlayer(PlayerId playerId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var player = uow.PlayerRepository.GetById(playerId);
			player.UpdateRankings(45, 67);

			uow.SaveChanges();
		}

		private static VenueId CreateVenue()
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var venueId = new VenueId(Guid.NewGuid());
			var wimbledon = Venue.Create(venueId, "Wimbledon", Surface.Grass);
			wimbledon.AddCourt(new CourtId(), "Centre Court", 14979);
			wimbledon.AddCourt(new CourtId(), "Court 1", 12345);

			var queens = Venue.Create(new VenueId(), "Queens Club", Surface.Grass);

			uow.VenueRepository.Add(wimbledon);
			uow.VenueRepository.Add(queens);

			uow.SaveChanges();

			return venueId;
		}

		private static void TestDuplicateCourtNames(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var venue = uow.VenueRepository.GetById(venueId);

			try
			{
				venue.AddCourt(new CourtId(), "Centre Court", 14979);
				uow.SaveChanges();
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static CourtId AddAnExtraCourt(VenueId venueId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var venue = uow.VenueRepository.GetById(venueId);
			var courtId = new CourtId();
			venue.AddCourt(courtId, "New Court", 100);
			uow.SaveChanges();

			return courtId;
		}

		private static void RemoveCourt(VenueId venueId, CourtId courtId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var venue = uow.VenueRepository.GetById(venueId);
			venue.RemoveCourt(courtId);
			uow.SaveChanges();
		}

		private static TournamentId CreateTournament(VenueId venueId, PlayerId playerId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var venue = uow.VenueRepository.GetById(venueId);
			var dates = new TournamentDates(new DateTime(2022, 07, 22), new DateTime(2022, 07, 29));
			var tournament = Tournament.Create((TournamentTitle)"Wimbledon 2022", TournamentLevel.GrandSlam,
				dates, venue);

			var eventSize = new EventSize(128, 32);
			tournament.AddEvent(EventType.MensSingles, eventSize, MatchFormat.FiveSetMatchWithFinalSetTieBreak);
			tournament.AddEvent(EventType.WomensSingles, eventSize, MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			tournament.OpenForEntries();

			var player = Player.Register(playerId, "Edward Entered", 12, 123, Gender.Male);
			tournament.EnterEvent(EventType.MensSingles, player);

			uow.TournamentRepository.Add(tournament);

			uow.SaveChanges();

			return tournament.Id;
		}

		private static void TestDuplicateEventTypes(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);

			try
			{
				var eventSize = new EventSize(128, 32);
				tournament.AddEvent(EventType.MensSingles, eventSize, MatchFormat.FiveSetMatchWithTwoGamesClear);
				uow.SaveChanges();
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static void AddTwoMorePlayersToEvents(TournamentId tournamentId, PlayerId playerId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var player = uow.PlayerRepository.GetById(playerId);

			var tournament = uow.TournamentRepository.GetById(tournamentId);

			var otherPlayer = Player.Register(new PlayerId(), "Brad New", 123, 121, Gender.Male);
			tournament.EnterEvent(EventType.WomensSingles, player);
			uow.PlayerRepository.Add(otherPlayer);
			tournament.EnterEvent(EventType.MensSingles, otherPlayer);

			uow.SaveChanges();
		}

		private static void TestAddingPlayersUsesLazyLoading(TournamentId tournamentId, PlayerId playerId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);

			try
			{
				tournament.EnterEvent(EventType.MensSingles, Player.Register(playerId, "Someone Else", 1, 1, Gender.Male));
			}
			catch
			{
				// Do nothing - just proving a point
			}
		}

		private static void ReadTournamentAndCloseEntries(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);
			tournament.CloseEntries();

			uow.SaveChanges();
		}

		private static void ReadEntries(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);

			var tennisEvent = tournament.GetEvent(EventType.MensSingles);
			var name = tennisEvent.Entries[0].PlayerOne.Name;
		}

		private static CompetitorId CreateCompetitor(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);
			var tennisEvent = tournament.GetEvent(EventType.MensSingles);
			var player = tennisEvent.Entries[0].PlayerOne;
			var competitor = Competitor.Create(tournament, EventType.MensSingles, new Seeding(1), player.Name);

			uow.CompetitorRepository.Add(competitor);
			uow.SaveChanges();

			return competitor.Id;
		}

		private static void ReadCompetitor(CompetitorId competitorId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var competitor = uow.CompetitorRepository.GetById(competitorId);
		}

		private static RoundId CreateRound(TournamentId tournamentId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var tournament = uow.TournamentRepository.GetById(tournamentId);

			var round = Round.Create(tournament, EventType.MensSingles, 1, 32);

			uow.RoundRepository.Add(round);
			uow.SaveChanges();

			return round.Id;
		}

		private static void ReadRound(RoundId roundId)
		{
			using var context = new TournamentManagementDbContext(_connectionString, _useConsoleLogger);
			var uow = new UnitOfWork(context);

			var round = uow.RoundRepository.GetById(roundId);
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
