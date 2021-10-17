namespace DomainDesign.Common
{
	public abstract class ValueObject<T> where T : ValueObject<T>
	{
		protected abstract bool ValueObjectEquals(T other);
		protected abstract int GetValueObjectHashCode();

		public override bool Equals(object obj)
		{
			var valueObject = obj as T;

			if (ReferenceEquals(valueObject, null)) return false;

			return ValueObjectEquals(valueObject);
		}

		public override int GetHashCode()
		{
			return GetValueObjectHashCode();
		}

		public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
		{
			if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;

			if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

			return a.Equals(b);
		}

		public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
		{
			return !(a == b);
		}
	}
}
