namespace TournamentManagement.Application.Repository
{
	public interface IUnitOfWork
	{
		ICompetitorRepository CompetitorRepository { get; }
		IPlayerRepository PlayerRepository { get; }
		IRoundRepository RoundRepository { get; }
		ITournamentRepository TournamentRepository { get; }
		IVenueRepository VenueRepository { get; }
		int SaveChanges();
	}
}
