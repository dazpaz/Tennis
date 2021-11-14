using System;
using System.Linq;

namespace DomainDesign.Common
{
	public static class Guard
	{
		public static void AgainstNullOrEmptyString(string value, string parameterName)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("Value can not be null or empty string", parameterName);
			}
		}

		public static void AgainstValueNotInSetOfAllowedValues<T>(T value, T[] allowedValues, string parameterName)
		{
			if (!allowedValues.Contains(value))
			{
				throw new ArgumentException($"{value} is not one of the allowed values", parameterName);
			}
		}

		public static void AgainstIntegerOutOfRange(int value, int lowLimit, int highLimit, string parameterName)
		{
			if (value < lowLimit || value > highLimit)
			{
				throw new ArgumentOutOfRangeException(parameterName, $"Value {value} must be between {lowLimit} and {highLimit}");
			}
		}

		public static void AgainstEmptyGuid(Guid guid, string parameterName)
		{
			if (guid == Guid.Empty)
			{
				throw new ArgumentException("Guid cannot have empty value", parameterName);
			}
		}
	}
}
