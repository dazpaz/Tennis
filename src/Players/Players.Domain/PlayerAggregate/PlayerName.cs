using CSharpFunctionalExtensions;

namespace Players.Domain.PlayerAggregate
{
	public class PlayerName : ValueObject<PlayerName>
	{
		public const int MaxNameLength = 50;

		public string Name { get; }

		private PlayerName(string name)
		{
			Name = name;
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
			return Name == other.Name;
		}

		protected override int GetHashCodeCore()
		{
			return Name.GetHashCode();
		}

		public static implicit operator string(PlayerName playerName)
		{
			return playerName.Name;
		}

		public static explicit operator PlayerName(string name)
		{
			return Create(name).Value;
		}
	}
}
