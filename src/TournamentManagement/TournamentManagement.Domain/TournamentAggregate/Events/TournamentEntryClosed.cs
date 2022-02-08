using DomainDesign.Common;

namespace TournamentManagement.Domain.TournamentAggregate.Events
{
	public sealed class TournamentEntryClosed : IDomainEvent
	{
		public TournamentId TournamentId { get; }

		public TournamentEntryClosed(TournamentId tournamentId)
		{
			TournamentId = tournamentId;
		}
	}
}
