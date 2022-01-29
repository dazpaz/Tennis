using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentManagement.Data.Configuration;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.RoundAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Data
{
	public class TournamentManagementDbContext : DbContext
	{
		private readonly string _connectionString;
		private readonly bool _useConsoleLogger;

		public DbSet<Tournament> Tournaments { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventEntry> EventEntries { get; set; }
		public DbSet<Competitor> Competitors { get; set; }
		public DbSet<Round> Rounds { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Venue> Venues { get; set; }
		public DbSet<Court> Courts { get; set; }

		public TournamentManagementDbContext(string connectionString, bool useConsoleLogger)
		{
			_connectionString = connectionString;
			_useConsoleLogger = useConsoleLogger;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter((category, level) =>
						category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
					.AddConsole();
			});

			optionsBuilder
				.UseSqlServer(_connectionString)
				.UseLazyLoadingProxies();

			if (_useConsoleLogger)
			{
				optionsBuilder
					.UseLoggerFactory(loggerFactory)
					.EnableSensitiveDataLogging();
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new TournamentEntityTypeConfiguration().Configure(modelBuilder.Entity<Tournament>());
			new EventEntityTypeConfiguration().Configure(modelBuilder.Entity<Event>());
			new EventEntryEntityTypeConfiguration().Configure(modelBuilder.Entity<EventEntry>());
			new PlayerEntityTypeConfiguration().Configure(modelBuilder.Entity<Player>());
			new VenueEntityTypeConfiguration().Configure(modelBuilder.Entity<Venue>());
			new CourtEntityTypeConfiguration().Configure(modelBuilder.Entity<Court>());
			new CompetitorEntityTypeConfiguration().Configure(modelBuilder.Entity<Competitor>());
			new RoundEntityTypeConfiguration().Configure(modelBuilder.Entity<Round>());
		}
	}
}
