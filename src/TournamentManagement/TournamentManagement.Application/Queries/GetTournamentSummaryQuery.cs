using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Queries
{
	public sealed class GetTournamentSummaryQuery : IQuery<List<TournamentSummaryDto>>
	{
	}

	public sealed class GetTournamentSummaryQueryHandler
		: IQueryHandler<GetTournamentSummaryQuery, List<TournamentSummaryDto>>
	{
		private readonly IUnitOfWork _uow;

		public GetTournamentSummaryQueryHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public List<TournamentSummaryDto> Handle(GetTournamentSummaryQuery query)
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
				TournamentLevel = tournament.Level,
				StartDate = tournament.StartDate,
				EndDate = tournament.EndDate,
				VenueName = tournament.Venue.Name
			};
		}
	}
}
