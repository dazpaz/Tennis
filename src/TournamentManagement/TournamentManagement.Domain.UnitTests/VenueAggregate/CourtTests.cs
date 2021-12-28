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
			var venueId = new VenueId();
			var court = Court.Create(courtId, "Centre Court", 14979, venueId);

			court.Id.Should().Be(courtId);
			court.Name.Should().Be("Centre Court");
			court.Capacity.Should().Be(14979);
			court.VenueId.Should().Be(venueId);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateACourtWithEmptyName(string name)
		{
			Action act = () => Court.Create(new CourtId(), name, 100, new VenueId());

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(name == null
					? "Value cannot be null. (Parameter 'name')"
					: "Required input name was empty. (Parameter 'name')");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(25000)]
		public void CanCreateACourtWithCapacityValuesAtTheLimitsOfTheValidRange(int capacity)
		{
			var court = Court.Create(new CourtId(), "Centre Court", capacity, new VenueId());

			court.Name.Should().Be("Centre Court");
			court.Capacity.Should().Be(capacity);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(25001)]
		public void CannotCreateCourtWithCapacityValuesOutsideTheValidRange(int capacity)
		{
			Action act = () => Court.Create(new CourtId(), "Centre Court", capacity, new VenueId());

			act.Should()
				.Throw<ArgumentException>();
		}
		
		[Fact]
		public void CanRenameACourt()
		{
			var court = Court.Create(new CourtId(), "Centre Court", 14979, new VenueId());

			court.RenameCourt("Murray Court");

			court.Name.Should().Be("Murray Court");
		}

		[Fact]
		public void CanUpdateTheCapacityOfACourt()
		{
			var court = Court.Create(new CourtId(), "Court 4", 100, new VenueId());

			court.UpdateCapacity(200);

			court.Capacity.Should().Be(200);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotRenameACourtWithEmptyName(string newName)
		{
			var court = Court.Create(new CourtId(), "Court 4", 100, new VenueId());

			Action act = () => court.RenameCourt(newName);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(newName == null
					? "Value cannot be null. (Parameter 'newName')"
					: "Required input newName was empty. (Parameter 'newName')");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(25001)]
		public void CannotUpdateCapacityWithValueOutsideTheValidRange(int capacity)
		{
			var court = Court.Create(new CourtId(), "Court 4", 100, new VenueId());

			Action act = () => court.UpdateCapacity(capacity);

			act.Should()
				.Throw<ArgumentException>();
		}
	}
}
