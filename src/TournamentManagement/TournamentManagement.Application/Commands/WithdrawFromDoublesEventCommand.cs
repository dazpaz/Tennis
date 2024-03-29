﻿using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class WithdrawFromDoublesEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public PlayerId PlayerOneId { get; }
		public PlayerId PlayerTwoId { get; }

		private WithdrawFromDoublesEventCommand(TournamentId tournamentId, EventType eventType,
			PlayerId playerOneId, PlayerId playerTwoId)
		{
			TournamentId = tournamentId;
			EventType = eventType;
			PlayerOneId = playerOneId;
			PlayerTwoId = playerTwoId;
		}

		public static Result<ICommand> Create(Guid tournamentId, EventType eventType,
			Guid playerOneId, Guid playerTwoId)
		{
			try
			{
				if (!Enum.IsDefined(typeof(EventType), eventType))
				{
					return Result.Failure<ICommand>("Invalid Event Type");
				}

				ICommand command = new WithdrawFromDoublesEventCommand(new TournamentId(tournamentId),
					eventType, new PlayerId(playerOneId), new PlayerId(playerTwoId));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<ICommand>(ex.Message);
			}
		}
	}

	public sealed class WithdrawFromDoublesEventCommandHandler : ICommandHandler<WithdrawFromDoublesEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public WithdrawFromDoublesEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(WithdrawFromDoublesEventCommand command)
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
				tournament.WithdrawFromDoublesEvent(command.EventType, playerOne, playerTwo);

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
