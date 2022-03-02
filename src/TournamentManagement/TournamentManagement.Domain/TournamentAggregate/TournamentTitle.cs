using Ardalis.GuardClauses;
using DomainDesign.Common;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class TournamentTitle : ValueObject<TournamentTitle>
	{
		public const int MaxLength = 50;

		public string Title { get; }

		public TournamentTitle(string title)
		{
			Guard.Against.NullOrWhiteSpace(title, nameof(title));
			Guard.Against.OutOfRange(title.Length, nameof(title), 1, MaxLength,
				$"Tournament title was too long, maximum length is {MaxLength}");

			Title = title;
		}

		public static implicit operator string(TournamentTitle title)
		{
			return title.Title;
		}

		public static explicit operator TournamentTitle(string title)
		{
			return new TournamentTitle(title);
		}

		public override string ToString()
		{
			return Title;
		}
	}
}
