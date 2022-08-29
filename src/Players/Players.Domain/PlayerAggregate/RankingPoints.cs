using CSharpFunctionalExtensions;


namespace Players.Domain.PlayerAggregate
{
	public sealed class RankingPoints : ValueObject<RankingPoints>
	{
		public const int MinPointsValue = 0;
		public const int MaxPointsValue = 20000;

		public int Points { get; }

		private RankingPoints(int points) : base()
		{
			Points = points;
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
			return Points == other.Points;
		}

		protected override int GetHashCodeCore()
		{
			return Points.GetHashCode();
		}

		public static implicit operator int(RankingPoints rankingPoints)
		{
			return rankingPoints.Points;
		}

		public static explicit operator RankingPoints(int points)
		{
			return Create(points).Value;
		}
	}
}
