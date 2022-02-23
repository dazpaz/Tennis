using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Commands
{
	public class AmendTournamentCommand : ICommand
	{
		public TournamentId Id { get; }
		public string Title { get; }
		public TournamentLevel TournamentLevel { get; }
		public DateTime StartDate { get; }
		public DateTime EndDate { get; }
		public VenueId VenueId { get; }

		public AmendTournamentCommand(Guid tournamentGuid, string title, TournamentLevel level, DateTime startDate,
			DateTime endDate, Guid venueGuid)
		{
			Id = new TournamentId(tournamentGuid);
			Title = title;
			TournamentLevel = level;
			StartDate = startDate;
			EndDate = endDate;
			VenueId = new VenueId(venueGuid);
		}
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
				tournament.AmendDetails(command.Title, command.TournamentLevel, 
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
