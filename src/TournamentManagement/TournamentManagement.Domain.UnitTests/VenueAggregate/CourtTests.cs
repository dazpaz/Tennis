using FluentAssertions;
using System;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.VenueAggregate
{
	public class CourtTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateCourtAndItIsCreatedCorrectly()
		{
			var courtId = new CourtId();
			var court = Court.Create(courtId, "Centre Court", 14979);

			court.Id.Should().Be(courtId);
			court.Name.Should().Be("Centre Court");
			court.Capacity.Should().Be(14979);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotCreateACourtWithEmptyName(string name)
		{
			Action act = () => Court.Create(new CourtId(), name, 100);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'name')");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(25000)]
		public void CanCreateACourtWithCapacityValuesAtTheLimitsOfTheValidRange(int capacity)
		{
			var court = Court.Create(new CourtId(), "Centre Court", capacity);

			court.Name.Should().Be("Centre Court");
			court.Capacity.Should().Be(capacity);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(25001)]
		public void CannotCreateCourtWithCapacityValuesOutsideTheValidRange(int capacity)
		{
			Action act = () => Court.Create(new CourtId(), "Centre Court", capacity);

			act.Should()
				.Throw<ArgumentException>();
		}
		
		[Fact]
		public void CanRenameACourt()
		{
			var court = Court.Create(new CourtId(), "Centre Court", 14979);

			court.RenameCourt("Murray Court");

			court.Name.Should().Be("Murray Court");
		}

		[Fact]
		public void CanUpdateTheCapacityOfACourt()
		{
			var court = Court.Create(new CourtId(), "Court 4", 100);

			court.UpdateCapacity(200);

			court.Capacity.Should().Be(200);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotRenameACourtWithEmptyName(string newName)
		{
			var court = Court.Create(new CourtId(), "Court 4", 100);

			Action act = () => court.RenameCourt(newName);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'newName')");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(25001)]
		public void CannotUpdateCapacityWithValueOutsideTheValidRange(int capacity)
		{
			var court = Court.Create(new CourtId(), "Court 4", 100);

			Action act = () => court.UpdateCapacity(capacity);

			act.Should()
				.Throw<ArgumentException>();
		}
	}
}
