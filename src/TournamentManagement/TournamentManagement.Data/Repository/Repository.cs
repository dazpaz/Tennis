using DomainDesign.Common;
using Microsoft.EntityFrameworkCore;
using TournamentManagement.Application.Repository;

namespace TournamentManagement.Data.Repository
{
	public class Repository<TAggregate, TKey> : IRepository<TAggregate, TKey>
		where TAggregate : class, IAggregateRoot
		where TKey : class
	{
		protected readonly DbSet<TAggregate> _dbSet;

		public Repository(TournamentManagementDbContext context)
		{
			_dbSet = context.Set<TAggregate>();
		}

		public virtual void Add(TAggregate entity)
		{
			_dbSet.Add(entity);
		}

		public virtual TAggregate GetById(TKey id)
		{
			var round = _dbSet.Find(id);
			return round;
		}
	}
}
