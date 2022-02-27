using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Queries
{
	public sealed class GetTournamentDetails : IQuery<TournamentDetailsDto>
	{
		public TournamentId Id { get; }

		public GetTournamentDetails(Guid tournamentId)
		{
			Id = new TournamentId(tournamentId);
		}
	}

	public sealed class GetTournamentDetailsHandler
		: IQueryHandler<GetTournamentDetails, TournamentDetailsDto>
	{
		private readonly IUnitOfWork _uow;

		public GetTournamentDetailsHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public TournamentDetailsDto Handle(GetTournamentDetails query)
		{
			var tournament = _uow.TournamentRepository.GetById(query.Id);
			return Convert.ToTournamentDetailsDto(tournament);
		}
	}
}
