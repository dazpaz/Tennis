using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TournamentManagement.Domain
{
	public class Tournament : Entity<Guid>
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

		private Tournament(Guid id) : base(id)
		{
			_events = new Dictionary<EventType, Event>();
			Events = new ReadOnlyDictionary<EventType, Event>(_events);
		}

		public static Tournament Create(string title, TournamentLevel level, DateTime startDate, DateTime endDate)
		{
			Guard.ForNullOrEmptyString(title, "title");

			var tournament = new Tournament(Guid.NewGuid())
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

		private void GuardForActionInCorrectState(TournamentState state, string action)
		{
			if (State != state)
			{
				throw new Exception($"Action {action} not allowed for a tournament if the state {state}");
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
	}
}
