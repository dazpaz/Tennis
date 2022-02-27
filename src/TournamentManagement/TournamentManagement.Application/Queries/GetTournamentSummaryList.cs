using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Queries
{
	public sealed class GetTournamentSummaryList : IQuery<List<TournamentSummaryDto>>
	{
	}

	public sealed class GetTournamentSummaryListHandler
		: IQueryHandler<GetTournamentSummaryList, List<TournamentSummaryDto>>
	{
		private readonly IUnitOfWork _uow;

		public GetTournamentSummaryListHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public List<TournamentSummaryDto> Handle(GetTournamentSummaryList query)
		{
			return _uow.TournamentRepository
				.GetList()
				.Select(t => ConvertToDto(t))
				.ToList();
		}

		private static TournamentSummaryDto ConvertToDto(Tournament tournament)
		{
			return new TournamentSummaryDto
			{
				Id = tournament.Id.Id,
				Title = tournament.Title,
				TournamentLevel = tournament.Level.ToString(),
				State = tournament.State.ToString(),
				StartDate = tournament.StartDate,
				EndDate = tournament.EndDate,
				VenueName = tournament.Venue.Name,
				NumberOfEvents = tournament.Events.Count
			};
		}
	}
}
