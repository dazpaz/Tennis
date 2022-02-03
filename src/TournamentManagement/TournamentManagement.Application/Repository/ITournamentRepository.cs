using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface ITournamentRepository
	{
		Tournament GetById(TournamentId id);
		void Add(Tournament tournament);
	}
}
