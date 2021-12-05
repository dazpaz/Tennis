using System;
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
	}
}
