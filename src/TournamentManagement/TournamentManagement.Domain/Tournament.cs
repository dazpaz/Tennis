using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class Tournament : Entity<TournamentId>
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
			Guard.AgainstNullOrEmptyString(title, nameof(title));

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
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "UpdateDetails");
			Guard.AgainstNullOrEmptyString(title, "title");

			Title = title;
			Level = level;
			Dates = new TournamentDates(startDate, endDate);
		}

		public void AddEvent(Event tennisEvent)
		{
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "AddEvent");
			GuardAgainstDuplicateEventType(tennisEvent.EventType);

			_events.Add(tennisEvent.EventType, tennisEvent);
		}

		public void RemoveEvent(EventType eventType)
		{
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "RemoveEvent");
			GuardAgainstMissingEventType(eventType);

			_events.Remove(eventType);
		}

		public void ClearEvents()
		{
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "ClearEvents");

			_events.Clear();
		}

		public void SetEvents(IEnumerable<Event> events)
		{
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "SetEvents");

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
			GuardAgainstActionInWrongState(TournamentState.BeingDefined, "OpenForEntries");
			GuardAgainstNoEvents();
			
			// Raise event to get notifications out to players telling them they can enter

			TransitionToState(TournamentState.AcceptingEntries);
		}

		public void CloseEntries()
		{
			GuardAgainstActionInWrongState(TournamentState.AcceptingEntries, "CloseEntries");

			// Raise event to get notification out to players saying if they are in or not

			TransitionToState(TournamentState.EntriesClosed);
		}

		public void DrawTheEvents()
		{
			GuardAgainstActionInWrongState(TournamentState.EntriesClosed, "DrawTheEvents");

			// Raise event to perform the draw for each event 

			TransitionToState(TournamentState.DrawComplete);
		}

		public void StartTournament()
		{
			GuardAgainstActionInWrongState(TournamentState.DrawComplete, "StartTournament");

			// Need to think about this one

			TransitionToState(TournamentState.InProgress);
		}

		public void EventCompleted(EventType eventType)
		{
			GuardAgainstActionInWrongState(TournamentState.InProgress, "EventCompleted");
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

		private void GuardAgainstActionInWrongState(TournamentState state, string action)
		{
			if (State != state)
			{
				throw new Exception($"Action {action} not allowed for a tournament in the state {State}");
			}
		}

		private void GuardAgainstDuplicateEventType(EventType eventType)
		{
			if (_events.ContainsKey(eventType))
			{
				throw new Exception($"Tournament already has an event of type {eventType}");
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
