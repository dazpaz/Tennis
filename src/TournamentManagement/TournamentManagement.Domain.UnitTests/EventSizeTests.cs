using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class EventSizeTests
	{
		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(8)]
		[InlineData(16)]
		[InlineData(32)]
		public void CanCreateAnEventSizeWithValidNumberOfSeeds(int numberofSeeds)
		{
			var eventNumbers = new EventSize(32, numberofSeeds);

			eventNumbers.EntrantsLimit.Should().Be(32);
			eventNumbers.NumberOfSeeds.Should().Be(numberofSeeds);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(8)]
		[InlineData(16)]
		[InlineData(32)]
		public void TheEntrantsLimitMustBeAtLeastTheNumberOfSeeds(int numberofSeeds)
		{
			var eventNumbers = new EventSize(numberofSeeds, numberofSeeds);

			eventNumbers.EntrantsLimit.Should().Be(numberofSeeds);
			eventNumbers.NumberOfSeeds.Should().Be(numberofSeeds);
		}

		[Fact]
		public void TheEntrantsLimitCanBeUpto128()
		{
			var eventNumbers = new EventSize(128, 32);

			eventNumbers.EntrantsLimit.Should().Be(128);
		}

		[Theory]
		[InlineData(2, 1)]
		[InlineData(3, 2)]
		[InlineData(4, 2)]
		[InlineData(5, 3)]
		[InlineData(8, 3)]
		[InlineData(9, 4)]
		[InlineData(16, 4)]
		[InlineData(17, 5)]
		[InlineData(32, 5)]
		[InlineData(33, 6)]
		[InlineData(64, 6)]
		[InlineData(65, 7)]
		[InlineData(128, 7)]
		public void TheNumberOfRoundsIsCalculatedBasedOnTheEntrantsLimit(int entrantsLimit,
			int expectedRounds)
		{
			var eventNumbers = new EventSize(entrantsLimit, 2);

			eventNumbers.NumberOfRounds.Should().Be(expectedRounds);
		}

		[Theory]
		[InlineData(2, 2)]
		[InlineData(4, 2)]
		[InlineData(8, 4)]
		[InlineData(16, 8)]
		[InlineData(32, 16)]
		[InlineData(64, 32)]
		[InlineData(128, 64)]
		public void TheMinimumEntrantsIsCalculatedBasedOnTheNumberOfRounds(int entrantsLimit,
			int expectedMinimum)
		{
			var eventNumbers = new EventSize(entrantsLimit, 2);

			eventNumbers.MinimumEntrants.Should().Be(expectedMinimum);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(9)]
		[InlineData(5)]
		public void TheNumberOfSeedsMustNotBeAnInvalidValue(int numberOfSeeds)
		{
			Action act = () => new EventSize(128, numberOfSeeds);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"{numberOfSeeds} is not one of the allowed values (Parameter 'numberOfSeeds')");
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(3, 4)]
		[InlineData(7, 8)]
		[InlineData(15, 16)]
		[InlineData(31, 32)]
		public void EntrantsLimitMustNotBeLessThanTheTheNumberOfSeeds(int entrantsLimit,
			int numberOfSeeds)
		{
			Action act = () => new EventSize(entrantsLimit, numberOfSeeds);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"Value {entrantsLimit} must be between {numberOfSeeds} and 128 (Parameter 'entrantsLimit')");
		}
	}
}
