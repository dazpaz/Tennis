using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class VenueTests
	{
		[Theory]
		[InlineData(Surface.Hard)]
		[InlineData(Surface.Grass)]
		[InlineData(Surface.Clay)]
		[InlineData(Surface.Carpet)]
		public void CanUseFactoryMethodToCreateVenueAndItIsCreatedCorrectly(Surface expectedSurface)
		{
			var value = Venue.Create("Flushing Meadows", expectedSurface);

			value.Id.Id.Should().NotBe(Guid.Empty);
			value.Name.Should().Be("Flushing Meadows");
			value.Surface.Should().Be(expectedSurface);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotCreateAVenueWithEmptyName(string name)
		{
			Action act = () => Venue.Create(name, Surface.Hard);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'name')");
		}
	}
}
