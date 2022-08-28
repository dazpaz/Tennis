using Players.Application.Repository;
using Players.Domain.CountryAggregate;
using Players.Domain.PlayerAggregate;

#nullable disable

namespace Players.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
	private readonly PlayersDbContext _context;

	IRepository<Player, PlayerId> _playerRepository;
	IRepository<Country, CountryId> _countryRepository;

	public IRepository<Player, PlayerId> PlayerRepository => GetPlayerRepository();
	public IRepository<Country, CountryId> CountryRepository => GetCountryRepository();

	public UnitOfWork(PlayersDbContext context)
	{
		_context = context;
	}

	private IRepository<Player, PlayerId> GetPlayerRepository()
	{
		if (_playerRepository == null)
		{
			_playerRepository = new Repository<Player, PlayerId>(_context);
		}
		return _playerRepository;
	}

	private IRepository<Country, CountryId> GetCountryRepository()
	{
		if (_countryRepository == null)
		{
			_countryRepository = new Repository<Country, CountryId>(_context);
		}
		return _countryRepository;
	}

	public int SaveChanges()
	{
		return _context.SaveChanges();
	}
}
