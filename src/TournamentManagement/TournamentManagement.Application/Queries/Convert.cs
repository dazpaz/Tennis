using System.Linq;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Queries
{
	public static class Convert
	{
		public static TournamentDetailsDto ToTournamentDetailsDto(Tournament tournament)
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
				Events = tournament.Events.Select(e => ToEventDto(e)).ToList()
			};
		}

		public static EventDto ToEventDto(Event tennisEvent)
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
