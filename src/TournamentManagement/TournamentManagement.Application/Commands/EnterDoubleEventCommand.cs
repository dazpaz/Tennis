using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class EnterDoublesEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public PlayerId PlayerOneId { get; }
		public PlayerId PlayerTwoId { get; }

		private EnterDoublesEventCommand(TournamentId tournamentId, EventType eventType,
			PlayerId playerOneId, PlayerId playerTwoId)
		{
			TournamentId = tournamentId;
			EventType = eventType;
			PlayerOneId = playerOneId;
			PlayerTwoId = playerTwoId;
		}

		public static Result<EnterDoublesEventCommand> Create(Guid tournamentId, EventType eventType,
			Guid playerOneId, Guid playerTwoId)
		{
			try
			{
				if (!Enum.IsDefined(typeof(EventType), eventType))
				{
					return Result.Failure<EnterDoublesEventCommand>("Invalid Event Type");
				}

				var command = new EnterDoublesEventCommand(new TournamentId(tournamentId), eventType,
					new PlayerId(playerOneId), new PlayerId(playerTwoId));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<EnterDoublesEventCommand>(ex.Message);
			}
		}
	}

	public sealed class EnterDoublesEventCommandHandler : ICommandHandler<EnterDoublesEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public EnterDoublesEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(EnterDoublesEventCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			var playerOne = _uow.PlayerRepository.GetById(command.PlayerOneId);
			if (playerOne == null)
			{
				return Result.Failure("Player one does not exist");
			}

			var playerTwo = _uow.PlayerRepository.GetById(command.PlayerTwoId);
			if (playerTwo == null)
			{
				return Result.Failure("Player two does not exist");
			}

			try
			{
				tournament.EnterDoublesEvent(command.EventType, playerOne, playerTwo);

				_uow.SaveChanges();
				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message);
			}
		}
	}
}
