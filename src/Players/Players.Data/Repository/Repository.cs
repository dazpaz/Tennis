using DomainDesign.Common;
using Microsoft.EntityFrameworkCore;
using Players.Application.Repository;

#nullable disable

namespace Players.Data.Repository;

public class Repository<TAggregate, TKey> : IRepository<TAggregate, TKey>
	where TAggregate : class, IAggregateRoot
	where TKey : class
{
	protected readonly DbSet<TAggregate> _dbSet;

	public Repository(PlayersDbContext context)
	{
		_dbSet = context.Set<TAggregate>();
	}

	public virtual void Add(TAggregate entity)
	{
		_dbSet.Add(entity);
	}

	public virtual TAggregate GetById(TKey id)
	{
		var entity = _dbSet.Find(id);
		return entity;
	}
}
