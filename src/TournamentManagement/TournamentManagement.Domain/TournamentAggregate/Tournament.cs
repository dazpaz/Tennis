﻿using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class Tournament : Entity<TournamentId>, IAggregateRoot
	{
		public string Title { get; private set; }
		public TournamentDates Dates { get; private set; }
		public TournamentState State { get; private set; }
		public TournamentLevel Level { get; private set; }
		public VenueId VenueId { get; private set; }

		public int Year => Dates.Year;
		public DateTime StartDate => Dates.StartDate;
		public DateTime EndDate => Dates.EndDate;

		public IReadOnlyDictionary<EventType, Event> Events { get; private set; }

		private readonly IDictionary<EventType, Event> _events;

		private Tournament(TournamentId id) : base(id)
		{
			_events = new Dictionary<EventType, Event>();
			Events = new ReadOnlyDictionary<EventType, Event>(_events);
		}

		public static Tournament Create(string title, TournamentLevel level,
			DateTime startDate, DateTime endDate, VenueId venueId)
		{
			Guard.Against.NullOrWhiteSpace(title, nameof(title));

			var tournament = new Tournament(new TournamentId())
			{
				Title = title,
				Level = level,
				State = TournamentState.BeingDefined,
				Dates = new TournamentDates(startDate, endDate),
				VenueId = venueId
			};

			return tournament;
		}

		public void UpdateDetails(string title, TournamentLevel level, DateTime startDate, DateTime endDate)
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(UpdateDetails));
			Guard.Against.NullOrWhiteSpace(title, nameof(title));

			Title = title;
			Level = level;
			Dates = new TournamentDates(startDate, endDate);
		}

		public void AddEvent(Event tennisEvent)
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(AddEvent));
			GuardAgainstDuplicateEventType(tennisEvent.EventType);
			GuardAgainstWrongTournamentId(tennisEvent.TournamentId);

			_events.Add(tennisEvent.EventType, tennisEvent);
		}

		public void RemoveEvent(EventType eventType)
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(RemoveEvent));
			GuardAgainstMissingEventType(eventType);

			_events.Remove(eventType);
		}

		public void ClearEvents()
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(ClearEvents));

			_events.Clear();
		}

		public void SetEvents(IEnumerable<Event> events)
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(SetEvents));

			_events.Clear();

			try
			{
				foreach (var tennisEvent in events)
				{
					GuardAgainstDuplicateEventType(tennisEvent.EventType);
					_events.Add(tennisEvent.EventType, tennisEvent);
				}
			}
			catch (Exception)
			{
				_events.Clear();
				throw;
			}
		}

		public void OpenForEntries()
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.BeingDefined, State, nameof(OpenForEntries));
			GuardAgainstNoEvents();
			
			// Raise event to get notifications out to players telling them they can enter

			TransitionToState(TournamentState.AcceptingEntries);
		}

		public void CloseEntries()
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.AcceptingEntries, State, nameof(CloseEntries));

			// Raise event to get notification out to players saying if they are in or not

			TransitionToState(TournamentState.EntriesClosed);
		}

		public void DrawTheEvents()
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.EntriesClosed, State, nameof(DrawTheEvents));

			// Raise event to perform the draw for each event 

			TransitionToState(TournamentState.DrawComplete);
		}

		public void StartTournament()
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.DrawComplete, State, nameof(StartTournament));

			// Need to think about this one

			TransitionToState(TournamentState.InProgress);
		}

		public void EventCompleted(EventType eventType)
		{
			Guard.Against.TornamentActionInWrongState(TournamentState.InProgress, State, nameof(EventCompleted));
			GuardAgainstMissingEventType(eventType);

			_events[eventType].MarkEventCompleted();

			if (_events.Values.All(e => e.IsCompleted))
			{
				TransitionToState(TournamentState.Complete);

				// Raise Event to update players details with their points and prize money
			}
		}

		private void TransitionToState(TournamentState newState)
		{
			State = newState;
		}

		private void GuardAgainstDuplicateEventType(EventType eventType)
		{
			if (_events.ContainsKey(eventType))
			{
				throw new Exception($"Tournament already has an event of type {eventType}");
			}
		}

		private void GuardAgainstWrongTournamentId(TournamentId tournamentId)
		{
			if (tournamentId != Id)
			{
				throw new Exception("Cannot add Event with the wrong Tournament Id");
			}
		}

		private void GuardAgainstMissingEventType(EventType eventType)
		{
			if (!_events.ContainsKey(eventType))
			{
				throw new Exception($"Tournament does not have an event of type {eventType}");
			}
		}

		private void GuardAgainstNoEvents()
		{
			if (_events.Count == 0)
			{
				throw new Exception($"Tournament must have at least one event to open it for entries");
			}
		}
	}
}
