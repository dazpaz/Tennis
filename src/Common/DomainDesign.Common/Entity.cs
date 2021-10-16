using System;

namespace DomainDesign.Common
{
	public abstract class Entity<T> : IEquatable<Entity<T>>
	{
		public T Id { get; protected set; }

		protected Entity(T id)
		{
			if (Equals(id, default(T)))
			{
				throw new ArgumentException("The ID cannot be the type's default value.", "id");
			}

			Id = id;
		}

		protected Entity()
		{
		}

		public bool Equals(Entity<T> other)
		{
			if (other == null) return false;

			return Id.Equals(other.Id);
		}

		public override bool Equals(object otherObject)
		{
			var other = otherObject as Entity<T>;

			if (other is null) return false;

			if (ReferenceEquals(this, other)) return true;

			if (GetType() != other.GetType()) return false;

			if (Equals(Id, default(T)) || Equals(other.Id, default(T))) return false;

			return Id.Equals(other.Id);
		}

		public static bool operator ==(Entity<T> a, Entity<T> b)
		{
			if (a is null && b is null) return true;

			if (a is null || b is null) return false;

			return a.Equals(b);
		}

		public static bool operator !=(Entity<T> a, Entity<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return (GetType().ToString() + Id).GetHashCode();
		}
	}
}
