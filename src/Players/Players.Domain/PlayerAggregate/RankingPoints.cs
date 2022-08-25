using CSharpFunctionalExtensions;


namespace Players.Domain.PlayerAggregate
{
	public sealed class RankingPoints : ValueObject<RankingPoints>
	{
		public const int MinPointsValue = 0;
		public const int MaxPointsValue = 20000;

		private int Value { get; }

		private RankingPoints(int value) : base()
		{
			Value = value;
		}

		public static Result<RankingPoints> Create(int points)
		{
			if (points < MinPointsValue || points > MaxPointsValue)
			{
				return Result.Failure<RankingPoints>(
					$"Rank Points value must be in the range {MinPointsValue} to {MaxPointsValue}, but was {points}");
			}

			return Result.Success(new RankingPoints(points));
		}

		protected override bool EqualsCore(RankingPoints other)
		{
			return Value == other.Value;
		}

		protected override int GetHashCodeCore()
		{
			return Value.GetHashCode();
		}

		public static implicit operator int(RankingPoints rankingPoints)
		{
			return rankingPoints.Value;
		}

		public static explicit operator RankingPoints(int points)
		{
			return Create(points).Value;
		}
	}
}
