using CSharpFunctionalExtensions;

namespace Players.Domain.PlayerAggregate
{
	public sealed class Height : ValueObject<Height>
	{
		public const int MinHeight = 120;
		public const int MaxHeight = 220;

		public int Value { get; }

		private Height(int value) : base()
		{
			Value = value;
		}

		public static Result<Height> Create(int height)
		{
			if (height < MinHeight || height > MaxHeight)
			{
				return Result.Failure<Height>(
					$"Height value must be in the range {MinHeight} to {MaxHeight}, but was {height}");
			}

			return Result.Success(new Height(height));
		}

		protected override bool EqualsCore(Height other)
		{
			return Value == other.Value;
		}

		protected override int GetHashCodeCore()
		{
			return Value.GetHashCode();
		}

		public static implicit operator int(Height height)
		{
			return height.Value;
		}

		public static explicit operator Height(int height)
		{
			return Create(height).Value;
		}
	}
}
