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
			var firstName = PlayerName.Create("Steve");
			var lastName = PlayerName.Create("Serve");
			var email = EmailAddress.Create("steve.server@tennis.com");
			var height = Height.Create(191);

			var player = Player.Register(firstName.Value,
				lastName.Value, email.Value,
				Gender.Male, new DateTime(2000, 10, 01),
				Plays.RightHanded, height.Value,
				Country.Create("GBR", "Great Britain"));

			player.Id.Id.Should().NotBe(Guid.Empty);
			player.FirstName.Should().Be(firstName.Value);
			player.LastName.Should().Be(lastName.Value);
			player.Email.Should().Be(email.Value);
			player.Gender.Should().Be(Gender.Male);
			player.DateOfBirth.Should().Be(new DateTime(2000, 10, 01));
			player.Plays.Should().Be(Plays.RightHanded);
			player.Height.Should().Be(height.Value);
			player.Country.FullName.Should().Be("Great Britain");
			player.SinglesRank.Should().Be(Ranking.Create(999).Value);
			player.DoublesRank.Should().Be(Ranking.Create(999).Value);
			player.SinglesRankingPoints.Should().Be(RankingPoints.Create(0).Value);
			player.DoublesRankingPoints.Should().Be(RankingPoints.Create(0).Value);
		}
	}
}
