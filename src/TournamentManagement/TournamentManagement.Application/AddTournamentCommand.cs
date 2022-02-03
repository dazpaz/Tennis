using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Application
{
	public sealed class AddTournamentCommand : ICommand
	{
		public string Title { get; set; }
		public int TournamentLevel { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid VenueId { get; set; }
	}

	public sealed class AddTournamentCommandHandler : ICommandHandler<AddTournamentCommand, Guid>
	{
		private ITournamentRepository _tournamentRepository;

		public AddTournamentCommandHandler(ITournamentRepository tournamentRepository)
		{
			_tournamentRepository = tournamentRepository;
		}

		public Result<Guid> Handle(AddTournamentCommand command)
		{
			// retrieve the Venue based on its ID
			var venue = Venue.Create(new VenueId(command.VenueId), "Roland Garros", Domain.Surface.Clay);

			var tournament = Tournament.Create(command.Title, TournamentLevel.GrandSlam,
					command.StartDate, command.EndDate, venue);

			// add tournament to the repository
			// save the repository

			return Result.Success(tournament.Id.Id);
		}
	}
}
