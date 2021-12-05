using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ardalis.GuardClauses
{
	public static class CommonGuards
	{
		public static int IntegerOutOfRange(this IGuardClause guardClause, int value, int lowLimit,
			int highLimit, string parameterName, string message = null)
		{
			if (value < lowLimit || value > highLimit)
			{
				throw new ArgumentOutOfRangeException(parameterName,
					message ?? $"Value {value} must be between {lowLimit} and {highLimit}");
			}

			return value;
		}

		public static Guid EmptyGuid(this IGuardClause guardClause, Guid guid, string parameterName,
			string message = null)
		{
			if (guid == Guid.Empty)
			{
				throw new ArgumentException(message ?? "Guid cannot have empty value", parameterName);
			}

			return guid;
		}

		public static T ValueNotInSetOfAllowedValues<T>(this IGuardClause guardClause,
			T value, T[] allowedValues, string parameterName)
		{
			if (!allowedValues.Contains(value))
			{
				throw new ArgumentException($"{value} is not one of the allowed values", parameterName);
			}

			return value;
		}

		public static void KeyValuesDoNotMatch<T>(this IGuardClause guardClause,
			T actualKey, T expectedKey, string message = null) where T : EntityId<T>
		{
			if (actualKey != expectedKey)
			{
				throw new Exception(message ?? "Cannot add Event with the wrong Tournament Id");
			}
		}

		public static void CollectionIsEmpty<T>(this IGuardClause guardClause, ICollection<T> collection,
			string message = null)
		{
			if (collection.Count == 0)
			{
				throw new Exception(message ?? "Collection does not contain any elements");
			}
		}

		public static void DictionaryAlreadyContainsKey<TKey, TValue>(this IGuardClause guardClause,
			IDictionary<TKey, TValue> dictionary, TKey key, string message = null)
		{
			if (dictionary.ContainsKey(key))
			{
				throw new Exception(message ?? $"Dictionary already contains key, {key}");
			}
		}

		public static void DictionaryDoesNotContainKey<TKey, TValue>(this IGuardClause guardClause,
			IDictionary<TKey, TValue> dictionary, TKey key, string message = null)
		{
			if (!dictionary.ContainsKey(key))
			{
				throw new Exception(message ?? $"Dictionary does not contain key, {key}");
			}
		}
	}
}
