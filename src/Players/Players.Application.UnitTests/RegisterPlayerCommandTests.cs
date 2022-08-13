using Players.Application.Commands;
using Players.Common;

namespace Players.Application.UnitTests;

public class RegisterPlayerCommandTests
{
	[Fact]
	public void CanCreateARegisterPlayerCommand()
	{
		var command = RegisterPlayerCommand.Create("Steve", "Serve", Gender.Male, new DateOnly(2000, 10, 01),
			Plays.LeftHanded, 191, "France");

		command.IsSuccess.Should().BeTrue();
		command.Value.FirstName.Should().Be("Steve");
		command.Value.LastName.Should().Be("Serve");
		command.Value.Gender.Should().Be(Gender.Male);
		command.Value.DateOfBirth.Should().Be(new DateOnly(2000, 10, 01));
		command.Value.Plays.Should().Be(Plays.LeftHanded);
		command.Value.Height.Should().Be(191);
		command.Value.Country.Should().Be("France");
	}

	[Fact]
	public void WhenCreatingARegisterPlayerCommandWithBadGenderThenErrorResultIsReturned()
	{
		var command = RegisterPlayerCommand.Create("Steve", "Serve", (Gender)5, new DateOnly(2000, 10, 01),
			Plays.LeftHanded, 191, "France");

		command.IsFailure.Should().BeTrue();
		command.Error.Should().Be("Invalid gender");
	}

	[Fact]
	public void WhenCreatingARegisterPlayerCommandWithBadPlaysThenErrorResultIsReturned()
	{
		var command = RegisterPlayerCommand.Create("Steve", "Serve", Gender.Female, new DateOnly(2000, 10, 01),
			(Plays)3, 191, "France");

		command.IsFailure.Should().BeTrue();
		command.Error.Should().Be("Invalid plays");
	}
}
