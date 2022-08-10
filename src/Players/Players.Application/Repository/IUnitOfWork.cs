using Players.Domain.PlayerAggregate;

namespace Players.Application.Repository;

public interface IUnitOfWork
{
	IRepository<Player, PlayerId> PlayerRepository { get; }
}
