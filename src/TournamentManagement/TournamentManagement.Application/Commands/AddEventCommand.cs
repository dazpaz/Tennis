using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class AddEventCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public EventType EventType { get; }
		public int EntrantsLimit { get; }
		public int NumberOfSeeds { get; }
		public int NumberOfSets { get; }
		public SetType FinalSetType { get; }

		public AddEventCommand(Guid tournamentId, EventType eventType, int entrantsLimit,
			int numberOfSeeds, int numberOfSets, SetType finalSetType)
		{
			TournamentId = new TournamentId(tournamentId);
			EventType = eventType;
			EntrantsLimit = entrantsLimit;
			NumberOfSeeds = numberOfSeeds;
			NumberOfSets = numberOfSets;
			FinalSetType = finalSetType;
		}
	}

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
				var matchFormat = new MatchFormat(command.NumberOfSets, command.FinalSetType);
				tournament.AddEvent(command.EventType, command.EntrantsLimit, command.NumberOfSeeds, matchFormat);

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
