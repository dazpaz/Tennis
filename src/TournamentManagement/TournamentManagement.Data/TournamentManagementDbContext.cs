using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;
using EventId = TournamentManagement.Domain.TournamentAggregate.EventId;

namespace TournamentManagement.Data
{
	public class TournamentManagementDbContext : DbContext
	{
		private readonly string _connectionString;
		private readonly bool _useConsoleLogger;

		public DbSet<Tournament> Tournaments { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventEntry> EventEntries { get; set; }
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
		}
	}

	public class TournamentEntityTypeConfiguration : IEntityTypeConfiguration<Tournament>
	{
		public void Configure(EntityTypeBuilder<Tournament> builder)
		{
			builder.ToTable("Tournament").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new TournamentId(p));
			builder.Property(p => p.Title)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.State);
			builder.Property(p => p.Level);
			builder.OwnsOne(p => p.Dates, p =>
			{
				p.Property(pp => pp.StartDate).HasColumnName("StartDate");
				p.Property(pp => pp.EndDate).HasColumnName("EndDate");
			});

			builder.HasOne(p => p.Venue).WithMany();

			builder.HasMany(b => b.Events).WithOne()
				.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}

	public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
	{
		public void Configure(EntityTypeBuilder<Event> builder)
		{
			builder.ToTable("Event").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new EventId(p));
			builder.Property(p => p.EventType);
			builder.Property(p => p.IsCompleted);
			builder.Ignore(p => p.SinglesEvent);

			builder.OwnsOne(p => p.EventSize, p =>
			{
				p.Property(pp => pp.EntrantsLimit).HasColumnName("EntrantsLimit");
				p.Property(pp => pp.NumberOfSeeds).HasColumnName("NumberOfSeeds");
			});

			builder.OwnsOne(p => p.MatchFormat, p =>
			{
				p.Property(pp => pp.NumberOfSets).HasColumnName("NumberOfSets");
				p.Property(pp => pp.FinalSetType).HasColumnName("FinalSetType");
			});
		}
	}

	public class EventEntryEntityTypeConfiguration : IEntityTypeConfiguration<EventEntry>
	{
		public void Configure(EntityTypeBuilder<EventEntry> builder)
		{
			builder.ToTable("EventEntry").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new EventEntryId(p));
			builder.Property(p => p.EventType);
			builder.Property(p => p.Rank);
			builder.Property(p => p.EventId)
				.HasConversion(p => p.Id, p => new EventId(p));
			builder.Ignore(p => p.Players);
		}
	}

	public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
	{
		public void Configure(EntityTypeBuilder<Player> builder)
		{
			builder.ToTable("Player").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new PlayerId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.SinglesRank);
			builder.Property(p => p.DoublesRank);
			builder.Property(p => p.Gender);
		}
	}

	public class VenueEntityTypeConfiguration : IEntityTypeConfiguration<Venue>
	{
		public void Configure(EntityTypeBuilder<Venue> builder)
		{
			builder.ToTable("Venue").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new VenueId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.Surface);
			builder.HasMany(b => b.Courts).WithOne()
				.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}

	public class CourtEntityTypeConfiguration : IEntityTypeConfiguration<Court>
	{
		public void Configure(EntityTypeBuilder<Court> builder)
		{
			builder.ToTable("Court").HasKey(k => k.Id);
			builder.Property(p => p.Id)
				.HasConversion(p => p.Id, p => new CourtId(p));
			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(p => p.Capacity);
		}
	}
}
