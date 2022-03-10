using System;
using TournamentManagement.Common;

namespace TournamentManagement.Contract
{
	public class EventDto
	{
		public Guid Id { get; set; }
		public EventType EventType { get; set; }
		public bool IsSinglesEvent { get; set; }
		public int NumberOfSets { get;  set; }
		public SetType FinalSetType { get; set; }
		public int EntrantsLimit { get; set; }
		public int NumberOfEntrants { get; set; }
		public bool IsCompleted { get; set; }
		public Guid TournamentId { get; set; }
	}
}
