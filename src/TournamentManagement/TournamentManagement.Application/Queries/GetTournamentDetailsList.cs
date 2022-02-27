using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;

namespace TournamentManagement.Application.Queries
{
	public sealed class GetTournamentDetailsList : IQuery<List<TournamentDetailsDto>>
	{

	}

	public sealed class GetTournamentDetailsListHandler
		: IQueryHandler<GetTournamentDetailsList, List<TournamentDetailsDto>>
	{
		private readonly IUnitOfWork _uow;

		public GetTournamentDetailsListHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public List<TournamentDetailsDto> Handle(GetTournamentDetailsList query)
		{
			return _uow.TournamentRepository
				.GetList()
				.Select(t => Convert.ToTournamentDetailsDto(t))
				.ToList();
		}
	}
}
