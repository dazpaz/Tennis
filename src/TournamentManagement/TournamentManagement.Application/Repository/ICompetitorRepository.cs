using TournamentManagement.Domain.CompetitorAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface ICompetitorRepository
	{
		Competitor GetById(CompetitorId id);
		void Add(Competitor competitor);
	}
}
