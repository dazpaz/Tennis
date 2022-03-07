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
	public sealed class AmendEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public EventSize EventSize { get; }
		public MatchFormat MatchFormat { get; }

		private AmendEventCommand(TournamentId tournamentId, EventType eventType, EventSize eventSize, MatchFormat matchFormat)
		{
			TournamentId = tournamentId;
			EventType = eventType;
			EventSize = eventSize;
			MatchFormat = matchFormat;
		}

		public static Result<AmendEventCommand> Create(Guid tournamentGuid, string eventType, int entrantsLimit,
				int numberOfSeeds, int numberOfSets, SetType finalSetType)
		{
			try
			{
				if (!Enum.TryParse(eventType, out EventType type))
				{
					return Result.Failure<AmendEventCommand>("Invalid event type");
				}

				if (!Enum.IsDefined(typeof(SetType), finalSetType))
				{
					return Result.Failure<AmendEventCommand>("Invalid set type");
				}

				var command = new AmendEventCommand(new TournamentId(tournamentGuid), type,
					new EventSize(entrantsLimit, numberOfSeeds), new MatchFormat(numberOfSets, finalSetType));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<AmendEventCommand>(ex.Message);
			}
		}
	}

	[Passthrough]
	[AuditCommand]
	public sealed class AmendEventCommandHandler : ICommandHandler<AmendEventCommand>
	{
		private readonly IUnitOfWork _uow;

		public AmendEventCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(AmendEventCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			try 
			{
				tournament.AmendEvent(command.EventType, command.EventSize, command.MatchFormat);

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
