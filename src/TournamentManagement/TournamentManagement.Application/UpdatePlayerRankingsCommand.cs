using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.PlayerAggregate;

namespace TournamentManagement.Application
{
	public class UpdatePlayerRankingsCommand : ICommand
	{
		public Guid Id { get; set; }
		public ushort SinglesRank { get; set; }
		public ushort DoublesRank { get; set; }
	}

	public sealed class UpdatePlayerRankingsCommandHandler : ICommandHandler<UpdatePlayerRankingsCommand>
	{
		private IPlayerRepository _playerRepository;

		public UpdatePlayerRankingsCommandHandler(IPlayerRepository tournamentRepository)
		{
			_playerRepository = tournamentRepository;
		}

		public Result Handle(UpdatePlayerRankingsCommand command)
		{
			var player = _playerRepository.GetById(new PlayerId(command.Id));
			if (player == null) return Result.Failure("Player Not Found");

			player.UpdateRankings(command.SinglesRank, command.DoublesRank);

			return Result.Success();
		}
	}
}
