using CSharpFunctionalExtensions;

namespace Players.Domain.PlayerAggregate
{
	public class PlayerName : ValueObject<PlayerName>
	{
		public const int MaxNameLength = 50;

		public string Value { get; }

		private PlayerName(string value)
		{
			Value = value;
		}

		public static Result<PlayerName> Create(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return Result.Failure<PlayerName>("Name should not be empty");
			}

			if (name.Length > MaxNameLength)
			{
				return Result.Failure<PlayerName>($"Name is too long, maximum length is {MaxNameLength}");
			}

			return Result.Success(new PlayerName(name));
		}

		protected override bool EqualsCore(PlayerName other)
		{
			return Value == other.Value;
		}

		protected override int GetHashCodeCore()
		{
			return Value.GetHashCode();
		}

		public static implicit operator string(PlayerName playerName)
		{
			return playerName.Value;
		}

		public static explicit operator PlayerName(string name)
		{
			return Create(name).Value;
		}
	}
}
