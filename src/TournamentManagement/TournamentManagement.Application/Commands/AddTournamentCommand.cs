using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class AddTournamentCommand : ICommand
	{
		public string Title { get; }
		public TournamentLevel TournamentLevel { get; }
		public DateTime StartDate { get; }
		public DateTime EndDate { get; }
		public VenueId VenueId { get; }

		public AddTournamentCommand(string title, TournamentLevel level, DateTime startDate,
			DateTime endDate, Guid venueGuid)
		{
			Title = title;
			TournamentLevel = level;
			StartDate = startDate;
			EndDate = endDate;
			VenueId = new VenueId(venueGuid);
		}
	}

	public sealed class AddTournamentCommandHandler : ICommandHandler<AddTournamentCommand, Guid>
	{
		private readonly IUnitOfWork _uow;

		public AddTournamentCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result<Guid> Handle(AddTournamentCommand command)
		{
			var venue = _uow.VenueRepository.GetById(command.VenueId);
			if (venue == null)
			{
				return Result.Failure<Guid>("Venue does not exist");
			}

			try
			{
				var tournament = Tournament.Create(command.Title, command.TournamentLevel,
					command.StartDate, command.EndDate, venue);

				_uow.TournamentRepository.Add(tournament);
				_uow.SaveChanges();

				return Result.Success(tournament.Id.Id);
			}
			catch (Exception ex)
			{
				return Result.Failure<Guid>(ex.Message);
			}
		}
	}
}
