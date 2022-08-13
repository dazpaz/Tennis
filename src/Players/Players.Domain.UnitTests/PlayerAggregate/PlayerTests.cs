using Players.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Domain.UnitTests.PlayerAggregate
{
	public class PlayerTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreatePlayerAndItIsCreatedCorrectly()
		{
			var player = Player.Register("Steve", "Serve", Gender.Male, new DateOnly(2000, 10, 01),
				Plays.RightHanded, 191, "United Kingdom");

			player.Id.Id.Should().NotBe(Guid.Empty);
			player.FirstName.Should().Be("Steve");
			player.LastName.Should().Be("Serve");
			player.FullName.Should().Be("Steve Serve");
			player.Gender.Should().Be(Gender.Male);
			player.DateOfBirth.Should().Be(new DateOnly(2000, 10, 01));
			player.Plays.Should().Be(Plays.RightHanded);
			player.Height.Should().Be(191);
			player.Country.Should().Be("United Kingdom");
			player.SinglesRank.Should().Be(999);
			player.DoublesRank.Should().Be(999);
			player.SinglesRankingPoints.Should().Be(0);
			player.DoublesRankingPoints.Should().Be(0);
		}
	}
}
