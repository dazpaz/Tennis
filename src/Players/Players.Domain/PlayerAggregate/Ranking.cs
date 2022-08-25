using CSharpFunctionalExtensions;

namespace Players.Domain.PlayerAggregate;

public sealed class Ranking : ValueObject<Ranking>
{
	public const int MinRankValue = 1;
	public const int MaxRankValue = 999;

	private int Value { get; }

	private Ranking(int value) : base()
	{
		Value = value;
	}

	public static Result<Ranking> Create(int rank)
	{
		if (rank < MinRankValue || rank > MaxRankValue)
		{
			return Result.Failure<Ranking>(
				$"Rank value must be in the range {MinRankValue} to {MaxRankValue}, but was {rank}");
		}

		return Result.Success(new Ranking(rank));
	}

	protected override bool EqualsCore(Ranking other)
	{
		return Value == other.Value;
	}

	protected override int GetHashCodeCore()
	{
		return Value.GetHashCode();
	}

	public static implicit operator int(Ranking ranking)
	{
		return ranking.Value;
	}

	public static explicit operator Ranking(int rank)
	{
		return Create(rank).Value;
	}
}
