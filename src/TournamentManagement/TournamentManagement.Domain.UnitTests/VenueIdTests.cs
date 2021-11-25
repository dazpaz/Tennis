using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class VenueIdTests
	{
		[Fact]
		public void CanCreateVenueIdWithNoParameters()
		{
			var venueId = new VenueId();

			venueId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateVenueIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var venueId = new VenueId(id);

			venueId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateVenueIdUsingAnEmptyGuid()
		{
			Action act = () => new VenueId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
