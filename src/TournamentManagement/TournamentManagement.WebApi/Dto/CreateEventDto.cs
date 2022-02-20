using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.WebApi.Dto
{
	public class CreateEventDto
	{
		public EventType EventType { get; set; }
		public int EntrantsLimit { get; set; }
		public int NumberOfSeeds { get; set; }
		public int NumberOfSets { get; set; }
		public SetType FinalSetType { get; set; }
	}
}
