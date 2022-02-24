namespace TournamentManagement.Contract
{
	public class AmendEventDto
	{
		public int EntrantsLimit { get; set; }
		public int NumberOfSeeds { get; set; }
		public int NumberOfSets { get; set; }
		public SetType FinalSetType { get; set; }
	}
}
