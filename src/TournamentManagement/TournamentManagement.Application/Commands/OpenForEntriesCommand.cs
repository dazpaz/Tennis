using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class OpenForEntriesCommand : ICommand
	{
		public TournamentId TournamentId { get; }

		private OpenForEntriesCommand(TournamentId tournamentId)
		{
			TournamentId = tournamentId;
		}

		public static Result<OpenForEntriesCommand> Create(Guid tournamentGuid)
		{
			try
			{
				var command = new OpenForEntriesCommand(new TournamentId(tournamentGuid));
				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<OpenForEntriesCommand>(ex.Message);
			}
			
		}
	}

	public sealed class OpenForEntriesCommandHandler : ICommandHandler<OpenForEntriesCommand>
	{
		private readonly IUnitOfWork _uow;

		public OpenForEntriesCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(OpenForEntriesCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			try
			{
				tournament.OpenForEntries();

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
