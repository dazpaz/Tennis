using TournamentManagement.Domain.RoundAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface IRoundRepository
	{
		Round GetById(RoundId id);
		void Add(Round round);
	}
}
