using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Players.Domain.PlayerAggregate
{
	public class EmailAddress : ValueObject<EmailAddress>
	{
		public const int MaxEmailLength = 256;

		public string Email { get; }

		private EmailAddress(string email)
		{
			Email = email;
		}

		public static Result<EmailAddress> Create(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return Result.Failure<EmailAddress>("Email should not be empty");
			}

			if (email.Length > MaxEmailLength)
			{
				return Result.Failure<EmailAddress>($"Email is too long, maximum length is {MaxEmailLength}");
			}

			// ToDo: validation of e-mail is minimal at the moment
			if (!email.Contains('@'))
			{
				return Result.Failure<EmailAddress>("Email is invalid");
			}

			return Result.Success(new EmailAddress(email));
		}

		protected override bool EqualsCore(EmailAddress other)
		{
			return Email == other.Email;
		}

		protected override int GetHashCodeCore()
		{
			return Email.GetHashCode();
		}

		public static implicit operator string(EmailAddress email)
		{
			return email.Email;
		}

		public static explicit operator EmailAddress(string email)
		{
			return Create(email).Value;
		}
	}
}
