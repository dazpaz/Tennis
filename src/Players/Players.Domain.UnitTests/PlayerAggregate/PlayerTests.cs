using Players.Common;
using Players.Domain.CountryAggregate;
using Players.Domain.PlayerAggregate;

namespace Players.Domain.UnitTests.PlayerAggregate
{
	public class PlayerTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreatePlayerAndItIsCreatedCorrectly()
		{
			var player = Player.Register(PlayerName.Create("Steve").Value,
				PlayerName.Create("Serve").Value,
				EmailAddress.Create("steve.server@tennis.com").Value,
				Gender.Male, new DateTime(2000, 10, 01),
				Plays.RightHanded, Height.Create(191).Value,
				Country.Create("GBR", "Great Britain"));

			player.Id.Id.Should().NotBe(Guid.Empty);
			player.FirstName.Should().Be("Steve");
			player.LastName.Should().Be("Serve");
			player.Gender.Should().Be(Gender.Male);
			player.DateOfBirth.Should().Be(new DateTime(2000, 10, 01));
			player.Plays.Should().Be(Plays.RightHanded);
			player.Height.Should().Be(191);
			player.Country.FullName.Should().Be("Great Britain");
			player.SinglesRank.Should().Be(999);
			player.DoublesRank.Should().Be(999);
			player.SinglesRankingPoints.Should().Be(0);
			player.DoublesRankingPoints.Should().Be(0);
		}
	}
}
