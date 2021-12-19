using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.PlayerAggregate.Repository;

namespace TournamentManagement.Application
{
	public sealed class RegisterPlayerCommand : ICommand
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public ushort SinglesRank { get; set; }
		public ushort DoublesRank { get; set; }
		public Gender Gender { get; set; }
	}

	public sealed class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand>
	{
		private IPlayerRepository _playerRepository;

		public RegisterPlayerCommandHandler(IPlayerRepository tournamentRepository)
		{
			_playerRepository = tournamentRepository;
		}

		public Result Handle(RegisterPlayerCommand command)
		{
			var playerId = new PlayerId(command.Id);
			var player = Player.Register(playerId, command.Name, command.SinglesRank,
				command.DoublesRank, command.Gender);

			// add player to the repository
			// save the repository (or unit of work)

			return Result.Success();
		}
	}
}
