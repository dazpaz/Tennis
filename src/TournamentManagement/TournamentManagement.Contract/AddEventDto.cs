using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class AddEventDto
	{
		public EventType EventType { get; set; }
		public int EntrantsLimit { get; set; }
		public int NumberOfSeeds { get; set; }
		public int NumberOfSets { get; set; }
		public SetType FinalSetType { get; set; }
	}
}
