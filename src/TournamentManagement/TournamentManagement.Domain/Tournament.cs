using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class Tournament : Entity<Guid>
	{
		public string Title { get; private set; }
		public TournamentDates Dates { get; private set; }
		public TournamentState State { get; private set; }

		public int Year => Dates.Year;
		public DateTime StartDate => Dates.StartDate;
		public DateTime EndDate => Dates.EndDate;

		private Tournament(Guid id) : base(id)
		{
		}

		public static Tournament Create(string title, DateTime startDate, DateTime endDate)
		{
			Guard.ForNullOrEmptyString(title, "title");

			var tournament = new Tournament(Guid.NewGuid())
			{
				Title = title,
				State = TournamentState.BeingDefined,
				Dates = new TournamentDates(startDate, endDate)
			};

			return tournament;
		}
	}
}
