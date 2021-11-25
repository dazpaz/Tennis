using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class Court : Entity<CourtId>
	{
		public string Name { get; private set; }
		public int Capacity { get; private set; } 

		private Court(CourtId id) : base(id)
		{
		}

		public static Court Create(string name, int capacity)
		{
			Guard.AgainstNullOrEmptyString(name, nameof(name));
			Guard.AgainstIntegerOutOfRange(capacity, 0, 20000, nameof(capacity));

			var court = new Court(new CourtId())
			{
				Name = name,
				Capacity = capacity
			};

			return court;
		}

		public void UpdateCapacity(int newCapacity)
		{
			Guard.AgainstIntegerOutOfRange(newCapacity, 0, 20000, nameof(newCapacity));
			Capacity = newCapacity;
		}

		public void RenameCourt(string newName)
		{
			Guard.AgainstNullOrEmptyString(newName, nameof(newName));
			Name = newName;
		}
	}
}
