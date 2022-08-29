using Players.Domain.CountryAggregate;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Repository;

public interface IUnitOfWork
{
	IRepository<Player, PlayerId> PlayerRepository { get; }
	IRepository<Country, CountryId> CountryRepository { get; }

	int SaveChanges();
}
