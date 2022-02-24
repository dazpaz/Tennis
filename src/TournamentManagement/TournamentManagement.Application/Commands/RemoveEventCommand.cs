using CSharpFunctionalExtensions;
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

		public RemoveEventCommand(Guid tournamentId, EventType eventType)
		{
			TournamentId = new TournamentId(tournamentId);
			EventType = eventType;
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
