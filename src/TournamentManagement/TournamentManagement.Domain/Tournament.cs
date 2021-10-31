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

		public static Tournament Create(string title, TournamentLevel level, DateTime startDate, DateTime endDate)
		{
			Guard.ForNullOrEmptyString(title, "title");

			var tournament = new Tournament(new TournamentId())
			{
				Title = title,
				Level = level,
				State = TournamentState.BeingDefined,
				Dates = new TournamentDates(startDate, endDate)
			};

			return tournament;
		}

		public void UpdateDetails(string title, TournamentLevel level, DateTime startDate, DateTime endDate)
		{
			GuardForActionInCorrectState(TournamentState.BeingDefined, "UpdateDetails");
			Guard.ForNullOrEmptyString(title, "title");

			Title = title;
			Level = level;
			Dates = new TournamentDates(startDate, endDate);
		}

		public void AddEvent(Event tennisEvent)
		{
			GuardForActionInCorrectState(TournamentState.BeingDefined, "AddEvent");
			GuardForDuplicateEventType(tennisEvent.EventType);

			_events.Add(tennisEvent.EventType, tennisEvent);
		}

		public void RemoveEvent(EventType eventType)
		{
			GuardForActionInCorrectState(TournamentState.BeingDefined, "RemoveEvent");
			GuardForMissingEventType(eventType);

			_events.Remove(eventType);
		}

		public void ClearEvents()
		{
			GuardForActionInCorrectState(TournamentState.BeingDefined, "ClearEvents");

			_events.Clear();
		}

		public void SetEvents(IEnumerable<Event> events)
		{
			GuardForActionInCorrectState(TournamentState.BeingDefined, "SetEvents");

			_events.Clear();

			try
			{
				foreach (var tennisEvent in events)
				{
					GuardForDuplicateEventType(tennisEvent.EventType);
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
			GuardForActionInCorrectState(TournamentState.BeingDefined, "OpenForEntries");
			GuardForAtLeastOneEvent();
			
			// Raise event to get notifications out to players telling them they can enter

			TransitionToState(TournamentState.AcceptingEntries);
		}

		public void CloseEntries()
		{
			GuardForActionInCorrectState(TournamentState.AcceptingEntries, "CloseEntries");

			// Raise event to get notification out to players saying if they are in or not

			TransitionToState(TournamentState.EntriesClosed);
		}

		public void DrawTheEvents()
		{
			GuardForActionInCorrectState(TournamentState.EntriesClosed, "DrawTheEvents");

			// Raise event to perform the draw for each event 

			TransitionToState(TournamentState.DrawComplete);
		}

		public void StartTournament()
		{
			GuardForActionInCorrectState(TournamentState.DrawComplete, "StartTournament");

			// Need to think about this one

			TransitionToState(TournamentState.InProgress);
		}

		public void EventCompleted(EventType eventType)
		{
			GuardForActionInCorrectState(TournamentState.InProgress, "EventCompleted");
			GuardForMissingEventType(eventType);

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

		private void GuardForActionInCorrectState(TournamentState state, string action)
		{
			if (State != state)
			{
				throw new Exception($"Action {action} not allowed for a tournament in the state {State}");
			}
		}

		private void GuardForDuplicateEventType(EventType eventType)
		{
			if (_events.ContainsKey(eventType))
			{
				throw new Exception($"Tournament already has an event of type {eventType}");
			}
		}

		private void GuardForMissingEventType(EventType eventType)
		{
			if (!_events.ContainsKey(eventType))
			{
				throw new Exception($"Tournament does not have an event of type {eventType}");
			}
		}

		private void GuardForAtLeastOneEvent()
		{
			if (_events.Count == 0)
			{
				throw new Exception($"Tournament must have at least one event to open it for entries");
			}
		}
	}
}
