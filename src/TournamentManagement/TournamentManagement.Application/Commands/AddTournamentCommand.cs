using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Decorators;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Commands
{
	public sealed class AddTournamentCommand : ICommand
	{
		public TournamentTitle Title { get; }
		public TournamentLevel Level { get; }
		public TournamentDates Dates { get; }
		public VenueId VenueId { get; }

		private AddTournamentCommand(TournamentTitle title, TournamentLevel level,
			TournamentDates tournamentDates, VenueId venueId)
		{
			Title = title;
			Level = level;
			Dates = tournamentDates;
			VenueId = venueId;
		}

		public static Result<AddTournamentCommand> Create(string title, TournamentLevel level,
			DateTime startDate, DateTime endDate, Guid venueGuid)
		{
			try
			{
				if (!Enum.IsDefined(typeof(TournamentLevel), level))
				{
					return Result.Failure<AddTournamentCommand>("Invalid tournament level");
				}

				var command = new AddTournamentCommand(new TournamentTitle(title), level,
					new TournamentDates(startDate, endDate), new VenueId(venueGuid));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<AddTournamentCommand>(ex.Message);
			}
		}
	}

	[Passthrough]
	[AuditCommand]
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
				var tournament = Tournament.Create(command.Title, command.Level,
					command.Dates, venue);

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
