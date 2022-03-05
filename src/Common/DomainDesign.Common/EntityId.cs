using Ardalis.GuardClauses;
using System;

namespace DomainDesign.Common
{
	public abstract class EntityId<T> : IEquatable<T> where T : EntityId<T>
	{
		public readonly Guid Id;

		public EntityId()
		{
			Id = Guid.NewGuid();
		}

		public EntityId(Guid id)
		{
			Guard.Against.EmptyGuid(id, nameof(id));
			Id = id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			var other = obj as T;
			return Equals(other);
		}

		public virtual bool Equals(T other)
		{
			if (other is null) return false;
			return other.Id == Id;
		}

		public override int GetHashCode()
		{
			return (GetType().ToString() + Id).GetHashCode();
		}

		public static bool operator ==(EntityId<T> a, EntityId<T> b)
		{
			if (a is null && b is null) return true;
			if (a is null || b is null) return false;
			return a.Equals(b);
		}

		public static bool operator !=(EntityId<T> a, EntityId<T> b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return Id.ToString();
		}
	}
}
