using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class PlayerTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreatePlayerAndItIsCreatedCorrectly()
		{
			var player = Player.Create("Steve Serve", 10, 200, Gender.Male);

			player.Id.Id.Should().NotBe(Guid.Empty);
			player.Name.Should().Be("Steve Serve");
			player.SinglesRank.Should().Be(10);
			player.DoublesRank.Should().Be(200);
			player.Gender.Should().Be(Gender.Male);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotCreateAPlayerWithEmptyName(string name)
		{
			Action act = () => Player.Create(name, 100, 200, Gender.Female);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'name')");
		}

		[Theory]
		[InlineData(1)]
		[InlineData(9999)]
		public void CanCreateAPlayerWithRankValuesAtTheLimitsOfTheValidRange(ushort rank)
		{
			var player = Player.Create("Steve Serve", rank, rank, Gender.Male);

			player.SinglesRank.Should().Be(rank);
			player.DoublesRank.Should().Be(rank);
		}

		[Theory]
		[InlineData(0, 100)]
		[InlineData(100, 0)]
		[InlineData(10000, 100)]
		[InlineData(100, 10000)]
		public void CanCreatePlayerWithRankValuesOutsideTheValidRange(ushort singlesRank, ushort doublesRank)
		{
			Action act = () => Player.Create("Steve Serve", singlesRank, doublesRank, Gender.Female);

			act.Should()
				.Throw<ArgumentException>();
		}
	}
}
