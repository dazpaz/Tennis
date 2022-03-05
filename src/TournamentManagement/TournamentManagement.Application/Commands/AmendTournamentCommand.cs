using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Decorators;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application.Commands
{
	public class AmendTournamentCommand : ICommand
	{
		public TournamentId TournamentId { get; }
		public TournamentTitle Title { get; }
		public TournamentLevel Level { get; }
		public TournamentDates Dates { get; }
		public VenueId VenueId { get; }

		private AmendTournamentCommand(TournamentId tournamentId, TournamentTitle title, TournamentLevel level,
			TournamentDates dates, VenueId venueId)
		{
			TournamentId = tournamentId;
			Title = title;
			Level = level;
			Dates = dates;
			VenueId = venueId;
		}

		public static Result<AmendTournamentCommand> Create(Guid tournamentGuid, string title, TournamentLevel level,
			DateTime startDate, DateTime endDate, Guid venueGuid)
		{
			try
			{
				if (!Enum.IsDefined(typeof(TournamentLevel), level))
				{
					return Result.Failure<AmendTournamentCommand>("Invalid tournament level");
				}

				var command = new AmendTournamentCommand(new TournamentId(tournamentGuid), new TournamentTitle(title),
					level, new TournamentDates(startDate, endDate), new VenueId(venueGuid));

				return Result.Success(command);
			}
			catch (Exception ex)
			{
				return Result.Failure<AmendTournamentCommand>(ex.Message);
			}
		}
	}

	[Passthrough]
	[AuditCommand]
	public sealed class AmendTournamentCommandHandler : ICommandHandler<AmendTournamentCommand>
	{
		private readonly IUnitOfWork _uow;

		public AmendTournamentCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(AmendTournamentCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.TournamentId);
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
				tournament.AmendDetails((TournamentTitle)command.Title, command.Level, 
					command.Dates, venue);

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
