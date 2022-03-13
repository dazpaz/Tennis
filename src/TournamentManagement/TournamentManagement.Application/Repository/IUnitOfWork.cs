using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.RoundAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface IUnitOfWork
	{
		IRepository<Competitor, CompetitorId> CompetitorRepository { get; }
		IRepository<Player, PlayerId> PlayerRepository { get; }
		IRepository<Round, RoundId> RoundRepository { get; }
		IRepository<Tournament, TournamentId> TournamentRepository { get; }
		IRepository<Venue, VenueId> VenueRepository { get; }

		int SaveChanges();
	}
}
