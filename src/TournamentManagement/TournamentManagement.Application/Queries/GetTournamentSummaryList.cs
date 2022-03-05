using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;

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
				.Select(t => Convert.ToTournamentSummaryDto(t))
				.ToList();
		}
	}
}
