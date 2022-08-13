using DomainDesign.Common;

namespace Players.Application.Repository;

public interface IRepository<TAggregate, TKey>
	where TAggregate : class, IAggregateRoot
	where TKey : class // constraint to EntityId
{
	TAggregate GetById(TKey id);
	void Add(TAggregate player);
}
