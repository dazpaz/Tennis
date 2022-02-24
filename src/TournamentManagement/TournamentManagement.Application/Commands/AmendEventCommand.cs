using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class AmendEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public int EntrantsLimit { get; }
		public int NumberOfSeeds { get; }
		public MatchFormat MatchFormat { get; }

		public AmendEventCommand(Guid tournamentId, EventType eventType, int entrantsLimit,
			int numberOfSeeds, int numberOfSets, SetType finalSetType)
		{
			TournamentId = new TournamentId(tournamentId);
			EventType = eventType;
			EntrantsLimit = entrantsLimit;
			NumberOfSeeds = numberOfSeeds;
			MatchFormat = new MatchFormat(numberOfSets, finalSetType);
		}
	}

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
				tournament.AmendEvent(command.EventType, command.EntrantsLimit, command.NumberOfSeeds,
					command.MatchFormat);

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
