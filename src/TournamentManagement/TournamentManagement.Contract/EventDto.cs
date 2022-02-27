namespace TournamentManagement.Contract
{
	public class EventDto
	{
		public string EventType { get; set; }
		public bool IsSinglesEvent { get; set; }
		public int NumberOfSets { get;  set; }
		public string FinalSet { get; set; }
		public int MinimumEntrants { get; set; }
		public int EntrantsLimit { get; set; }
		public int NumberEntrants { get; set; }
		public bool IsCompleted { get; set; }
	}
}
