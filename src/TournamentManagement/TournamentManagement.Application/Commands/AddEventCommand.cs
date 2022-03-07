using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Decorators;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class AddEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public EventSize EventSize { get; }
		public MatchFormat MatchFormat { get; }

		private AddEventCommand(TournamentId tournamentId, EventType eventType, EventSize eventSize, MatchFormat matchFormat)
		{
			TournamentId = tournamentId;
			EventType = eventType;
			EventSize = eventSize;
			MatchFormat = matchFormat;
		}


		public static Result<AddEventCommand> Create(Guid tournamentGuid, EventType eventType, int entrantsLimit,
				int numberOfSeeds, int numberOfSets, SetType finalSetType)
		{
			try
			{
				if (!Enum.IsDefined(typeof(EventType), eventType))
				{
					return Result.Failure<AddEventCommand>("Invalid event Type");
				}

				if (!Enum.IsDefined(typeof(SetType), finalSetType))
				{
					return Result.Failure<AddEventCommand>("Invalid set type");
				}

				var command = new AddEventCommand(new TournamentId(tournamentGuid), eventType,
					new EventSize(entrantsLimit, numberOfSeeds), new MatchFormat(numberOfSets, finalSetType));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<AddEventCommand>(ex.Message);
			}
		}
	}

	[Passthrough]
	[AuditCommand]
	public sealed class AddEventCommandHandler : ICommandHandler<AddEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public AddEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(AddEventCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			try
			{
				tournament.AddEvent(command.EventType, command.EventSize, command.MatchFormat);

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
