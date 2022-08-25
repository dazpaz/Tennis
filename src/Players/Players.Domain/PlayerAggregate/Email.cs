using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Players.Domain.PlayerAggregate
{
	public class Email : ValueObject<Email>
	{
		public const int MaxEmailLength = 256;

		public string Value { get; }

		private Email(string value)
		{
			Value = value;
		}

		public static Result<Email> Create(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return Result.Failure<Email>("Email should not be empty");
			}

			if (email.Length > MaxEmailLength)
			{
				return Result.Failure<Email>($"Email is too long, maximum length is {MaxEmailLength}");
			}

			if (!email.Contains("@"))
			{
				return Result.Failure<Email>("Email is invalid");
			}

			return Result.Success(new Email(email));
		}

		protected override bool EqualsCore(Email other)
		{
			return Value == other.Value;
		}

		protected override int GetHashCodeCore()
		{
			return Value.GetHashCode();
		}

		public static implicit operator string(Email email)
		{
			return email.Value;
		}

		public static explicit operator Email(string email)
		{
			return Create(email).Value;
		}
	}
}
