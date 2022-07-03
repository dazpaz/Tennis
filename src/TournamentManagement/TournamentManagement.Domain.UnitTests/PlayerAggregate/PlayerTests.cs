using FluentAssertions;
using System;
using TournamentManagement.Common;
using TournamentManagement.Domain.PlayerAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.PlayerAggregate
{
	public class PlayerTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreatePlayerAndItIsCreatedCorrectly()
		{
			var playerId = new PlayerId();
			var player = Player.Register(playerId, "Steve Serve", 10, 200, Gender.Male);

			player.Id.Should().Be(playerId);
			player.Name.Should().Be("Steve Serve");
			player.SinglesRank.Should().Be(10);
			player.DoublesRank.Should().Be(200);
			player.Gender.Should().Be(Gender.Male);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateAPlayerWithEmptyName(string name)
		{
			Action act = () => Player.Register(new PlayerId(), name, 100, 200, Gender.Female);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(name == null
				? "Value cannot be null. (Parameter 'name')"
				: "Required input name was empty. (Parameter 'name')");
		}

		[Theory]
		[InlineData(1)]
		[InlineData(9999)]
		public void CanCreateAPlayerWithRankValuesAtTheLimitsOfTheValidRange(ushort rank)
		{
			var player = Player.Register(new PlayerId(), "Steve Serve", rank, rank, Gender.Male);

			player.SinglesRank.Should().Be(rank);
			player.DoublesRank.Should().Be(rank);
		}

		[Theory]
		[InlineData(0, 100)]
		[InlineData(100, 0)]
		[InlineData(10000, 100)]
		[InlineData(100, 10000)]
		public void CannotCreatePlayerWithRankValuesOutsideTheValidRange(ushort singlesRank, ushort doublesRank)
		{
			Action act = () => Player.Register(new PlayerId(), "Steve Serve", singlesRank, doublesRank,
				Gender.Female);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("*Rank * is outside allowed range, 1 - 9999 (Parameter '*')");
		}

		[Theory]
		[InlineData(1)]
		[InlineData(9999)]
		public void CanUpdatePlayersRankWithValuesAtTheLimitsOfTheValidRange(ushort rank)
		{
			var player = Player.Register(new PlayerId(), "Doris Dropshot", 100, 200, Gender.Female);

			player.UpdateRankings(rank, rank);

			player.SinglesRank.Should().Be(rank);
			player.DoublesRank.Should().Be(rank);
		}

		[Theory]
		[InlineData(0, 100)]
		[InlineData(100, 0)]
		[InlineData(10000, 100)]
		[InlineData(100, 10000)]
		public void CannotUpdatePlayerRankWithRankValuesOutsideTheValidRange(ushort singlesRank,
			ushort doublesRank)
		{
			var player = Player.Register(new PlayerId(), "Doris Dropshot", 100, 200, Gender.Female);

			Action act = () => player.UpdateRankings(singlesRank, doublesRank);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("*Rank * is outside allowed range, 1 - 9999 (Parameter '*')");
		}
	}
}
