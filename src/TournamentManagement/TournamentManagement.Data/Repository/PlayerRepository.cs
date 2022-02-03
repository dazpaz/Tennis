using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Data.Repository
{
	public class PlayerRepository : IPlayerRepository
	{
		private readonly TournamentManagementDbContext _context;

		public PlayerRepository(TournamentManagementDbContext context)
		{
			_context = context;
		}

		public Player GetById(PlayerId id)
		{
			var player = _context.Players.Find(id);
			return player;
		}

		public void Add(Player player)
		{
			_context.Players.Add(player);
		}
	}
}
