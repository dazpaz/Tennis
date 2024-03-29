﻿using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentManagement.Common;
using TournamentManagement.Domain.Common;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.TournamentAggregate.Events;
using TournamentManagement.Domain.VenueAggregate;

[assembly: InternalsVisibleTo("TournamentManagement.Domain.UnitTests")]

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Tournament : AggregateRoot<TournamentId>
	{
		public TournamentTitle Title { get; private set; }
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

		public static Tournament Create(TournamentTitle title, TournamentLevel level,
			TournamentDates dates, Venue venue)
		{
			Guard.Against.Null(title, nameof(title));
			Guard.Against.Null(dates, nameof(dates));
			Guard.Against.Null(venue, nameof(venue));

			var tournament = new Tournament(new TournamentId())
			{
				Title = title,
				Level = level,
				State = TournamentState.BeingDefined,
				Dates = dates,
				Venue = venue
			};

			return tournament;
		}

		public void AmendDetails(TournamentTitle title, TournamentLevel level,
			TournamentDates dates, Venue venue)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(AmendDetails));
			Guard.Against.Null(title, nameof(title));
			Guard.Against.Null(dates, nameof(dates));
			Guard.Against.Null(venue, nameof(venue));

			Title = title;
			Level = level;
			Dates = dates;
			Venue = venue;
		}

		public Event GetEvent(EventType eventType)
		{
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);
			return tennisEvent;
		}

		public void AddEvent(EventType eventType, EventSize eventSize, MatchFormat matchFormat)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(AddEvent));
			Guard.Against.DuplicateEventType(_events, eventType);

			var tennisEvent = Event.Create(eventType, eventSize, matchFormat);

			_events.Add(tennisEvent);
		}

		public void AmendEvent(EventType eventType, EventSize eventSize, MatchFormat matchFormat)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.BeingDefined, State, nameof(AmendEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.AmendDetails(eventSize, matchFormat);
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

			TransitionToState(TournamentState.AcceptingEntries);
			RaiseDomainEvent(new TournamentEntryOpened(Id, _events.Select(e => e.EventType)));
		}

		public void EnterSinglesEvent(EventType eventType, Player playerOne)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State,
				nameof(EnterSinglesEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.EnterSinglesEvent(playerOne);
		}

		public void EnterDoublesEvent(EventType eventType, Player playerOne, Player playerTwo)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State,
				nameof(EnterDoublesEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.EnterDoublesEvent(playerOne, playerTwo);
		}

		public void WithdrawFromSinglesEvent(EventType eventType, Player playerOne)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State,
				nameof(WithdrawFromSinglesEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.WithdrawFromSinglesEvent(playerOne);
		}

		public void WithdrawFromDoublesEvent(EventType eventType, Player playerOne, Player playerTwo)
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State,
				nameof(WithdrawFromDoublesEvent));
			var tennisEvent = Guard.Against.MissingEventType(_events, eventType);

			tennisEvent.WithdrawFromDoublesEvent(playerOne, playerTwo);
		}

		public void CloseEntries()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.AcceptingEntries, State, nameof(CloseEntries));

			TransitionToState(TournamentState.EntriesClosed);
			RaiseDomainEvent(new TournamentEntryClosed(Id));
		}

		public void DrawTheEvents()
		{
			Guard.Against.TournamentActionInWrongState(TournamentState.EntriesClosed, State, nameof(DrawTheEvents));

			// Raise event to perform the draw for each event - or do it ourselves in here

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
