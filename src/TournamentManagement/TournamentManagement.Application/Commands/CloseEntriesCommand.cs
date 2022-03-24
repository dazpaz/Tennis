using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class CloseEntriesCommand : ICommand
	{
		public TournamentId TournamentId { get; }

		private CloseEntriesCommand(TournamentId tournamentId)
		{
			TournamentId = tournamentId;
		}

		public static Result<ICommand> Create(Guid tournamentGuid)
		{
			try
			{
				ICommand command = new CloseEntriesCommand(new TournamentId(tournamentGuid));
				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<ICommand>(ex.Message);
			}
		}
	}

	public sealed class CloseEntriesCommandHandler : ICommandHandler<CloseEntriesCommand>
	{
		private readonly IUnitOfWork _uow;

		public CloseEntriesCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(CloseEntriesCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			try
			{
				tournament.CloseEntries();

				_uow.SaveChanges();

				return Result.Success(tournament.Id);
			}
			catch (Exception ex)
			{
				return Result.Failure<TournamentId>(ex.Message);
			}
		}
	}
}
