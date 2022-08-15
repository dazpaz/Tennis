using Players.Application.Repository;
using Players.Domain.PlayerAggregate;

#nullable disable

namespace Players.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
	private readonly PlayersDbContext _context;

	IRepository<Player, PlayerId> _playerRepository;

	public IRepository<Player, PlayerId> PlayerRepository => GetPlayerRepository();

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

	public int SaveChanges()
	{
		return _context.SaveChanges();
	}
}
