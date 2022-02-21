using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application
{
	public class AmendTournamentCommand : ICommand
	{
		public TournamentId Id { get; set; }
		public string Title { get; set; }
		public TournamentLevel TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public VenueId VenueId { get; set; }
	}

	public sealed class AmendTournamentCommandHandler : ICommandHandler<AmendTournamentCommand>
	{
		private readonly IUnitOfWork _uow;

		public AmendTournamentCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(AmendTournamentCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.Id);
			if (tournament == null)
			{
				return Result.Failure("Tournament does not exist");
			}

			var venue = _uow.VenueRepository.GetById(command.VenueId);
			if (venue == null)
			{
				return Result.Failure<TournamentId>("Venue does not exist");
			}

			try
			{
				tournament.UpdateDetails(command.Title, command.TournamentLevel, 
					command.StartDate, command.EndDate, venue);

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
