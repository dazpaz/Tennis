using FluentAssertions;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentDatesTests
	{
		[Fact]
		public void CanCreateTournamentDatesUsingStartAndEndDates()
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10, 8, 9, 10), new DateTime(2020, 06, 15, 4, 5, 6));

			dates.StartDate.Should().Be(new DateTime(2020, 06, 10));
			dates.EndDate.Should().Be(new DateTime(2020, 06, 15));
			dates.Duration.Should().Be(6);
			dates.Year.Should().Be(2020);
		}

		[Fact]
		public void CanCreateTournamentDatesUsingStartDateAndDuration()
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10, 8, 9, 10), 10);

			dates.StartDate.Should().Be(new DateTime(2020, 06, 10));
			dates.EndDate.Should().Be(new DateTime(2020, 06, 19));
			dates.Duration.Should().Be(10);
			dates.Year.Should().Be(2020);
		}

		[Fact]
		public void TournamentYearIsTakenFromTheStartDateNotEndDate()
		{
			var dates = new TournamentDates(new DateTime(2020, 12, 26), 14);

			dates.Year.Should().Be(2020);
			dates.EndDate.Year.Should().Be(2021);
		}

		[Fact]
		public void CanCreateSingleDayTournament()
		{
			var dates = TournamentDates.CreateSingleDayTournament(new DateTime(2020, 06, 10));
			dates.StartDate.Should().Be(new DateTime(2020, 06, 10));
			dates.EndDate.Should().Be(new DateTime(2020, 06, 10));
			dates.Duration.Should().Be(1);
			dates.Year.Should().Be(2020);
		}

		[Theory]
		[InlineData(2018)]
		[InlineData(2051)]
		public void TournamentStartDateCannotBeForAnYearOutOfRange(int year)
		{
			Action act = () => new TournamentDates(new DateTime(year, 06, 10), 7);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"Value {year} must be between 2019 and 2050 (Parameter 'year')");
		}

		[Theory]
		[InlineData(2019)]
		[InlineData(2050)]
		public void TournamentStartDateCanBeBetweenYear2019And2050(int year)
		{
			var dates = new TournamentDates(new DateTime(year, 06, 10), 7);
			dates.Year.Should().Be(year);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(15)]
		public void TournamentDurationCannotBeOutsideTheRange1To14Days(int duration)
		{
			Action act = () => new TournamentDates(new DateTime(2020, 06, 10), duration);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Tournament duration must be 1 - 14 days");
		}

		[Theory]
		[InlineData(1)]
		[InlineData(14)]
		public void TournamentDurationCanBeBetween1And14Days(int duration)
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10), duration);
			dates.Duration.Should().Be(duration);
		}

		[Fact]
		public void CanChangeTheStartDateOfATournamentDates()
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10), 6);
			dates = dates.NewStartDate(new DateTime(2020, 06, 11));

			dates.StartDate.Day.Should().Be(11);
			dates.Duration.Should().Be(5);
		}

		[Fact]
		public void CanChangeTheEndDateOfATournamentDates()
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10), 6);
			dates = dates.NewEndDate(new DateTime(2020, 06, 11));

			dates.EndDate.Day.Should().Be(11);
			dates.Duration.Should().Be(2);
		}

		[Fact]
		public void CanChangeTheDurationOfATournamentDates()
		{
			var dates = new TournamentDates(new DateTime(2020, 06, 10), 6);
			dates = dates.NewDuration(2);

			dates.EndDate.Day.Should().Be(11);
			dates.Duration.Should().Be(2);
		}

		[Fact]
		public void IfTwoTournamentDatesHaveSameValuesTheySouldBeConsideredEqual()
		{
			var dates1 = new TournamentDates(new DateTime(2020, 06, 10), 6);
			var dates2 = new TournamentDates(new DateTime(2020, 06, 10), 6);

			dates1.Equals(dates2).Should().BeTrue();
			(dates1 == dates2).Should().BeTrue();
			(dates1 != dates2).Should().BeFalse();
		}

		[Fact]
		public void IfTwoTournamentDatesHaveDifferentValuesTheySouldBeConsideredNotEqual()
		{
			var dates1 = new TournamentDates(new DateTime(2020, 06, 10), 6);
			var dates2 = new TournamentDates(new DateTime(2020, 06, 10), 7);

			dates1.Equals(dates2).Should().BeFalse();
			(dates1 == dates2).Should().BeFalse();
			(dates1 != dates2).Should().BeTrue();
		}
	}
}
