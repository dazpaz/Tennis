namespace TournamentManagement.WebApi.Dto
{
	public class CreateEventDto
	{
		public int EventType { get; set; }
		public int EntrantsLimit { get; set; }
		public int NumberOfSeeds { get; set; }
		public int NumberOfSets { get; set; }
		public int FinalSetType { get; set; }
	}
}
