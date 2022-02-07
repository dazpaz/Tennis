using System.Collections.Generic;

namespace DomainDesign.Common
{
	public abstract class AggregateRoot<T> : Entity<T>
	{
		private readonly List<IDomainEvent> _domainEvents = new();
		public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

		protected void RaiseDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}

		protected void ClearDomainEvents()
		{
			_domainEvents.Clear();
		}

		protected AggregateRoot(T id) : base(id)
		{
		}

		protected AggregateRoot()
		{
		}
	}
}
