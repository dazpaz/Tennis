using Ardalis.GuardClauses;
using DomainDesign.Common;

namespace TournamentManagement.Domain.VenueAggregate
{
	public class Court : Entity<CourtId>
	{
		public const int MinCapacity = 0;
		public const int MaxCapacity = 25000;

		public string Name { get; private set; }
		public int Capacity { get; private set; }

		protected Court()
		{
		}

		private Court(CourtId id) : base(id)
		{
		}

		internal static Court Create(CourtId id, string name, int capacity)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));
			Guard.Against.IntegerOutOfRange(capacity, MinCapacity, MaxCapacity, nameof(capacity));

			var court = new Court(id)
			{
				Name = name,
				Capacity = capacity
			};

			return court;
		}

		public void UpdateCapacity(int newCapacity)
		{
			Capacity = Guard.Against.IntegerOutOfRange(newCapacity, MinCapacity, MaxCapacity, nameof(newCapacity));
		}

		public void RenameCourt(string newName)
		{
			Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName));
		}
	}
}
