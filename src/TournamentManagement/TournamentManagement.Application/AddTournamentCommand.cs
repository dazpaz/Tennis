using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application
{
	public sealed class AddTournamentCommand : ICommand
	{
		public string Title { get; set; }
		public TournamentLevel TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public VenueId VenueId { get; set; }
	}

	public sealed class AddTournamentCommandHandler : ICommandHandler<AddTournamentCommand, TournamentId>
	{
		private readonly IUnitOfWork _uow;

		public AddTournamentCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result<TournamentId> Handle(AddTournamentCommand command)
		{
			var venue = _uow.VenueRepository.GetById(command.VenueId);
			if (venue == null)
			{
				return Result.Failure<TournamentId>("Venue does not exist");
			}

			try
			{
				var tournament = Tournament.Create(command.Title, command.TournamentLevel,
					command.StartDate, command.EndDate, venue);

				_uow.TournamentRepository.Add(tournament);
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
