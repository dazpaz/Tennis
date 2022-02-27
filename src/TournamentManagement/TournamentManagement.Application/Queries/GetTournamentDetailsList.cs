using DomainDesign.Common;
using System.Collections.Generic;
using System.Linq;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

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
				.Select(t => ConvertToTournamentDetailsDto(t))
				.ToList();
		}

		private static TournamentDetailsDto ConvertToTournamentDetailsDto(Tournament tournament)
		{
			return new TournamentDetailsDto
			{
				Id = tournament.Id.Id,
				Title = tournament.Title,
				TournamentLevel = tournament.Level.ToString(),
				State = tournament.State.ToString(),
				StartDate = tournament.StartDate,
				EndDate = tournament.EndDate,
				VenueId = tournament.Venue.Id.Id,
				VenueName = tournament.Venue.Name,
				NumberOfEvents = tournament.Events.Count,
				Events = tournament.Events.Select(e => ConvertToEventDto(e)).ToList()
			};
		}

		private static EventDto ConvertToEventDto(Event tennisEvent)
		{
			return new EventDto
			{
				EventType = tennisEvent.EventType.ToString(),
				IsSinglesEvent = tennisEvent.SinglesEvent,
				NumberOfSets = tennisEvent.MatchFormat.NumberOfSets,
				FinalSet = tennisEvent.MatchFormat.FinalSetType.ToString(),
				MinimumEntrants = tennisEvent.EventSize.MinimumEntrants,
				EntrantsLimit = tennisEvent.EventSize.EntrantsLimit,
				NumberEntrants = tennisEvent.Entries.Count,
				IsCompleted = tennisEvent.IsCompleted
			};
		}
	}
}
