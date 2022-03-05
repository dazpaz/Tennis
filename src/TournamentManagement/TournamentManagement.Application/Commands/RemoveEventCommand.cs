﻿using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class RemoveEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }

		private RemoveEventCommand(TournamentId tournamentId, EventType eventType)
		{
			TournamentId = tournamentId;
			EventType = eventType;
		}

		public static Result<RemoveEventCommand> Create(Guid tournamentGuid, string eventType)
		{
			if (!Enum.TryParse(eventType, out EventType type))
			{
				return Result.Failure<RemoveEventCommand>("Invalid event type");
			}

			try
			{
				var command = new RemoveEventCommand(new TournamentId(tournamentGuid), type);
				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<RemoveEventCommand>(ex.Message);
			}
		}
	}

	public sealed class RemoveEventCommandHandler : ICommandHandler<RemoveEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public RemoveEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(RemoveEventCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			try
			{
				tournament.RemoveEvent(command.EventType);

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