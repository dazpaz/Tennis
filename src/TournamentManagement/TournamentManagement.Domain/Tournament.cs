using DomainDesign.Common;
using System;

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

		private Tournament(Guid id) : base(id)
		{
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
			if (State != TournamentState.BeingDefined)
			{
				throw new Exception("Can only change details of the Tournament before it is opened for entry.");
			}

			Guard.ForNullOrEmptyString(title, "title");

			Title = title;
			Level = level;
			Dates = new TournamentDates(startDate, endDate);
		}
	}
}
