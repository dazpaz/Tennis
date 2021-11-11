using FluentAssertions;
using System;
using Xunit;


namespace TournamentManagement.Domain.UnitTests
{
	public class SeedingTests
	{
		[Theory]
		[InlineData(1)]
		[InlineData(32)]
		public void CanCreateASeeding(int seed)
		{
			var seeding = new Seeding(seed);

			seeding.Seed.Should().Be(seed);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(33)]
		public void CannotCreateASeedingWithSeedValueOutOfRange(int seed)
		{
			Action act = () => new Seeding(seed);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage($"Value {seed} must be between 1 and 32 (Parameter 'seed')");
		}

		[Fact]
		public void SeedEqualityShouldWork()
		{
			var seeding1 = new Seeding(1);
			var seeding2 = new Seeding(1);
			var seeding3 = new Seeding(2);

			(seeding1 == seeding2).Should().BeTrue();
			(seeding1 == seeding3).Should().BeFalse();
		}
	}
}
