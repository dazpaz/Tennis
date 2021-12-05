using Ardalis.GuardClauses;
using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain.TournamentAggregate
{
	public class TournamentDates : ValueObject<TournamentDates>
	{
		private const int MinAllowedYear = 2019;
		private const int MaxAllowedYear = 2050;
		private const int MaxDurationInDays = 14;

		public DateTime StartDate { get; }
		public DateTime EndDate { get; }
		public int Year => StartDate.Year;
		public int Duration => (EndDate - StartDate).Days + 1;

		public TournamentDates(DateTime startDate, DateTime endDate)
		{
			Guard.Against.DurationOutOfRange(endDate.Date - startDate.Date, MaxDurationInDays);
			Guard.Against.IntegerOutOfRange(startDate.Year, MinAllowedYear, MaxAllowedYear, nameof(Year));

			StartDate = startDate.Date;
			EndDate = endDate.Date;
		}

		public TournamentDates(DateTime startDate, int duration)
			: this(startDate, startDate.AddDays(duration - 1))
		{
		}

		public TournamentDates NewEndDate(DateTime newEnd)
		{
			return new TournamentDates(StartDate, newEnd);
		}

		public TournamentDates NewDuration(int newDuration)
		{
			return new TournamentDates(StartDate, newDuration);
		}
		public TournamentDates NewStartDate(DateTime newStart)
		{
			return new TournamentDates(newStart, EndDate);
		}

		public static TournamentDates CreateSingleDayTournament(DateTime startDate)
		{
			return new TournamentDates(startDate, startDate);
		}
	}
}
