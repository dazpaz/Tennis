using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Application.Repository
{
	public interface IPlayerRepository
	{
		Player GetById(PlayerId id);
		void Add(Player player);
	}
}
