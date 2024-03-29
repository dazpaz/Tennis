﻿using FluentAssertions;
using System;
using System.Linq;
using TournamentManagement.Domain.VenueAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.VenueAggregate
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
			var venueId = new VenueId();
			var venue = Venue.Create(venueId, "Flushing Meadows", expectedSurface);

			venue.Id.Should().Be(venueId);
			venue.Name.Should().Be("Flushing Meadows");
			venue.Surface.Should().Be(expectedSurface);
			venue.Courts.Count.Should().Be(0);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateAVenueWithEmptyName(string name)
		{
			Action act = () => Venue.Create(new VenueId(), name, Surface.Hard);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(name == null
					? "Value cannot be null. (Parameter 'name')"
					: "Required input name was empty. (Parameter 'name')");
		}

		[Fact]
		public void CanAddCourtsToAVenue()
		{
			var venue = Venue.Create(new VenueId(), "Flushing Meadows", Surface.Hard);

			venue.AddCourt(new CourtId(), "Arthur Ashe", 23771);
			venue.AddCourt(new CourtId(), "Louis Armstrong", 14053);
			venue.AddCourt(new CourtId(), "Grandstand", 8125);

			venue.Courts.Count.Should().Be(3);
			var court = venue.Courts.First(c => c.Name == "Louis Armstrong");
			court.Capacity.Should().Be(14053);
		}

		[Fact]
		public void CannotAddCourtsWithTheSameNameToAVenue()
		{
			var venue = Venue.Create(new VenueId(), "Flushing Meadows", Surface.Hard);
			venue.AddCourt(new CourtId(), "Arthur Ashe", 23771);
			venue.Courts.Count.Should().Be(1);

			Action act = () => venue.AddCourt(new CourtId(), "Arthur Ashe", 100);

			act.Should()
				.Throw<Exception>()
				.WithMessage("Cannot add court to venue, court name already exists: Arthur Ashe");
		}

		[Fact]
		public void CanRemoveCourtsFromAVenue()
		{
			var venue = Venue.Create(new VenueId(), "Flushing Meadows", Surface.Hard);

			venue.AddCourt(new CourtId(), "Arthur Ashe", 23771);
			venue.AddCourt(new CourtId(), "Louis Armstrong", 14053);
			venue.AddCourt(new CourtId(), "Grandstand", 8125);
			venue.Courts.Count.Should().Be(3);
			var court = venue.Courts.First(c => c.Name == "Louis Armstrong");

			venue.RemoveCourt(court.Id);

			venue.Courts.Count.Should().Be(2);
		}
	}
}
