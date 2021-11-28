namespace TournamentManagement.Domain.TournamentAggregate
{
	public enum TournamentState
	{
		BeingDefined,
		AcceptingEntries,
		EntriesClosed,
		DrawComplete,
		InProgress,
		Complete
	}
}
