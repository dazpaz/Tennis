using System;
using System.Linq;

namespace DomainDesign.Common
{
	public static class Guard
	{
		public static void ForNullOrEmptyString(string value, string parameterName)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("Value can not be null or empty string", parameterName);
			}
		}

		public static void ForValueNotInSetOfAllowedValues<T>(T value, T[] allowedValues, string parameterName)
		{
			if (!allowedValues.Contains(value))
			{
				throw new ArgumentException($"{value} is not one of the allowed values", parameterName);
			}
		}

		public static void ForIntegerOutOfRange(int value, int lowLimit, int highLimit, string parameterName)
		{
			if (value < lowLimit || value > highLimit)
			{
				throw new ArgumentOutOfRangeException(parameterName, $"Value {value} must be between {lowLimit} and {highLimit}");
			}
		}
	}
}
