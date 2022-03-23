using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class EnterSinglesEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public PlayerId PlayerOneId { get; }

		private EnterSinglesEventCommand(TournamentId tournamentId, EventType eventType,
			PlayerId playerOneId)
		{
			TournamentId = tournamentId;
			EventType = eventType;
			PlayerOneId = playerOneId;
		}

		public static Result<EnterSinglesEventCommand> Create(Guid tournamentId, EventType eventType,
			Guid playerOneId)
		{
			try
			{
				if (!Enum.IsDefined(typeof(EventType), eventType))
				{
					return Result.Failure<EnterSinglesEventCommand>("Invalid Event Type");
				}

				var command = new EnterSinglesEventCommand(new TournamentId(tournamentId), eventType,
					new PlayerId(playerOneId));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<EnterSinglesEventCommand>(ex.Message);
			}
		}
	}

	public sealed class EnterSingleEventCommandHandler : ICommandHandler<EnterSinglesEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public EnterSingleEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(EnterSinglesEventCommand command)
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

			try
			{
				tournament.EnterSinglesEvent(command.EventType, playerOne);

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