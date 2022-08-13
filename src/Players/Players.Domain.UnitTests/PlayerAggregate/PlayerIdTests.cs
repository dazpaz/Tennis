using Players.Domain.PlayerAggregate;

namespace Players.Domain.UnitTests.PlayerAggregate;

public class PlayerIdTests
{
	[Fact]
	public void CanCreatePlayerIdWithNoParameters()
	{
		var playerId = new PlayerId();

		playerId.Id.Should().NotBe(Guid.Empty);
	}

	[Fact]
	public void CanCreatePlayerIdUsingASpecifiedGuid()
	{
		var id = Guid.NewGuid();
		var playerId = new PlayerId(id);

		playerId.Id.Should().Be(id);
	}

	[Fact]
	public void CannotCreatePlayerIdUsingAnEmptyGuid()
	{
		Action act = () => new PlayerId(Guid.Empty);

		act.Should()
			.Throw<ArgumentException>()
			.WithMessage("Guid cannot have empty value (Parameter 'id')");
	}
}
