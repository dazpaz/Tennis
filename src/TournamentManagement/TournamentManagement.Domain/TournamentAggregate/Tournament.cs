using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.VenueAggregate;

[assembly: InternalsVisibleTo("TournamentManagement.Domain.UnitTests")]

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Tournament : Entity<TournamentId>, IAggregateRoot
	{
		public string Title { get; private set; }
		public TournamentDates Dates { get; private set; }
		public TournamentState State { get; private set; }
		public TournamentLevel Level { get; private set; }
		public virtual Venue Venue { get; private set; }

		public int Year => Dates.Year;
		public DateTime StartDate => Dates.StartDate;
		public DateTime EndDate => Dates.EndDate;

		private readonly List<Event> _events = new();
		public virtual IReadOnlyList<Event> Events => _events.ToList();

		protected Tournament()
		{
		}

		private Tournament(TournamentId id) : base(id)
		{
		}

		public static Tournament Create(string title, TournamentLevel level,
			DateTime startDate, DateTime endDate, Venue venue)
		{
			Guard.Against.NullOrWhiteSpace(title, nameof(title));
			Guard.Against.Null(venue, nameof(venue));

			var tournament = new Tournament(new TournamentId())
			{
				Title = title,
				Level = level,
				State = TournamentState.BeingDefined,
				Dates = new TournamentDates(startDate, endDate),
				Venue = venue
			};

			return tournament;
		}

		public void UpdateDetails(string title, TournamentLevel level, DateTime startDate,
			DateTime endDate, Venue venue)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(UpdateDetails));
			Guard.Against.NullOrWhiteSpace(title, nameof(title));
			Guard.Against.Null(venue, nameof(venue));

			Title = title;
			Level = level;
			Dates = new TournamentDates(startDate, endDate);
			Venue = venue;
		}

		public Event GetEvent(EventType eventType)
		{
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);
			return tennisEvent;
		}

		public void AddEvent(EventType eventType, int entrantsLimit, int numberOfSeeds,
			int numberOfSets, SetType finalSetType)
		{
			var matchFormat = new MatchFormat(numberOfSets, finalSetType);
			AddEvent(eventType, entrantsLimit, numberOfSeeds, matchFormat);
		}

		public void AddEvent(EventType eventType, int entrantsLimit, int numberOfSeeds, MatchFormat matchFormat)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(AddEvent));
			Guard.Against.DuplicateEventType(_events, eventType);

			var tennisEvent = Event.Create(eventType, entrantsLimit, numberOfSeeds, matchFormat);

			_events.Add(tennisEvent);
		}

		public void RemoveEvent(EventType eventType)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(RemoveEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			_events.Remove(tennisEvent);
		}

		public void ClearEvents()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(ClearEvents));

			_events.Clear();
		}

		public void OpenForEntries()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(OpenForEntries));
			Guard.Against.NoEvents(_events);
			
			// Raise event to get notifications out to players telling them they can enter

			TransitionToState(TournamentState.AcceptingEntries);
		}

		public void EnterEvent(EventType eventType, Player playerOne, Player playerTwo = null)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State, nameof(EnterEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.EnterEvent(playerOne, playerTwo);
		}

		public void WithdrawFromEvent(EventType eventType, Player playerOne, Player playerTwo = null)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State, nameof(WithdrawFromEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.WithdrawFromEvent(playerOne, playerTwo);
		}

		public void CloseEntries()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State, nameof(CloseEntries));

			// Raise event to get notification out to players saying if they are in or not

			TransitionToState(TournamentState.EntriesClosed);
		}

		public void DrawTheEvents()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.EntriesClosed, State, nameof(DrawTheEvents));

			// Raise event to perform the draw for each event

			TransitionToState(TournamentState.DrawComplete);
		}

		public void StartTournament()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.DrawComplete, State, nameof(StartTournament));

			// Need to think about this one

			TransitionToState(TournamentState.InProgress);
		}

		public void CompleteEvent(EventType eventType)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.InProgress, State, nameof(CompleteEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.CompleteEvent();

			if (_events.All(e => e.IsCompleted))
			{
				TransitionToState(TournamentState.Complete);

				// Raise Event to update players details with their points and prize money
			}
		}

		private void TransitionToState(TournamentState newState)
		{
			State = newState;
		}
	}
}
