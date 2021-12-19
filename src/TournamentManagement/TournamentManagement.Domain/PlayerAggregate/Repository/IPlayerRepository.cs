using System;

namespace TournamentManagement.Domain.PlayerAggregate.Repository
{
	public interface IPlayerRepository
	{
		Player GetById(Guid id);
	}
}
