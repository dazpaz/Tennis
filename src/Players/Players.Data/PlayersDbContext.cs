using Cqrs.Common.Data;
using DomainDesign.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Players.Data.Configuration;
using Players.Domain.CountryAggregate;
using Players.Domain.PlayerAggregate;

#nullable disable

namespace Players.Data;

public class PlayersDbContext : DbContext
{
	private readonly string _connectionString;
	private readonly bool _useConsoleLogger;

	public DbSet<Player> Players { get; set; }
	public DbSet<Country> Countries { get; set; }

	public PlayersDbContext(ConnectionString connectionString, bool useConsoleLogger)
	{
		_connectionString = connectionString.Value;
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
		new PlayerEntityTypeConfiguration().Configure(modelBuilder.Entity<Player>());
		new CountryEntityTypeConfiguration().Configure(modelBuilder.Entity<Country>());
	}

	public override int SaveChanges()
	{
		List<IAggregateRoot> aggregateRoots = ChangeTracker
			.Entries()
			.Where(x => x.Entity is IAggregateRoot)
			.Select(x => (IAggregateRoot)x.Entity)
			.ToList();

		int result = base.SaveChanges();

		foreach (IAggregateRoot aggregateRoot in aggregateRoots)
		{
			Console.WriteLine($"Dispatching {aggregateRoot.DomainEvents.Count} Events for Aggregate Root of type {aggregateRoot.GetType()}");
			aggregateRoot.ClearDomainEvents();
		}

		return result;
	}
}
