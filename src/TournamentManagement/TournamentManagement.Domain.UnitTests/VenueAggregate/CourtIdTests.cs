using FluentAssertions;
using System;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.VenueAggregate
{
	public class CourtIdTests
	{
		[Fact]
		public void CanCreateCourtIdWithNoParameters()
		{
			var courtId = new CourtId();

			courtId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateCourtIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var courtId = new CourtId(id);

			courtId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateCourtIdUsingAnEmptyGuid()
		{
			Action act = () => new CourtId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
