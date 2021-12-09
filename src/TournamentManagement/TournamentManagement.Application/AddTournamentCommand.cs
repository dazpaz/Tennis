using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.TournamentAggregate.Repository;
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
			var tournament = Tournament.Create(command.Title, TournamentLevel.GrandSlam,
					command.StartDate, command.EndDate, new VenueId(command.VenueId));

			// add tournament to the repository
			// save the repository

			return Result.Success(tournament.Id.Id);
		}
	}
}
