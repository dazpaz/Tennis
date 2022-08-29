using CSharpFunctionalExtensions;

namespace Players.Domain.PlayerAggregate;

public sealed class Ranking : ValueObject<Ranking>
{
	public const int MinRankValue = 1;
	public const int MaxRankValue = 999;

	public int Rank { get; }

	private Ranking(int rank) : base()
	{
		Rank = rank;
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
		return Rank == other.Rank;
	}

	protected override int GetHashCodeCore()
	{
		return Rank.GetHashCode();
	}

	public static implicit operator int(Ranking ranking)
	{
		return ranking.Rank;
	}

	public static explicit operator Ranking(int rank)
	{
		return Create(rank).Value;
	}
}
