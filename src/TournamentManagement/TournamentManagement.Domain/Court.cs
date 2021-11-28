using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class Court : Entity<CourtId>
	{
		public const int MinCapacity = 0;
		public const int MaxCapacity = 25000;

		public string Name { get; private set; }
		public int Capacity { get; private set; } 

		private Court(CourtId id) : base(id)
		{
		}

		public static Court Create(CourtId id, string name, int capacity)
		{
			Guard.AgainstNullOrEmptyString(name, nameof(name));
			Guard.AgainstIntegerOutOfRange(capacity, MinCapacity, MaxCapacity, nameof(capacity));

			var court = new Court(id)
			{
				Name = name,
				Capacity = capacity
			};

			return court;
		}

		public void UpdateCapacity(int newCapacity)
		{
			Guard.AgainstIntegerOutOfRange(newCapacity, MinCapacity, MaxCapacity, nameof(newCapacity));
			Capacity = newCapacity;
		}

		public void RenameCourt(string newName)
		{
			Guard.AgainstNullOrEmptyString(newName, nameof(newName));
			Name = newName;
		}
	}
}
