﻿using System.Collections.Generic;

namespace DomainDesign.Common
{
	public interface IAggregateRoot
	{
		IReadOnlyList<IDomainEvent> DomainEvents { get; }
		void ClearDomainEvents();
	}

	public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
	{
		private readonly List<IDomainEvent> _domainEvents = new();
		public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

		protected void RaiseDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}

		public void ClearDomainEvents()
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
