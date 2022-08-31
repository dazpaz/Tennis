using Players.Application.Commands;
using Players.Common;

namespace Players.Application.UnitTests;

public class RegisterPlayerCommandTests
{
	[Fact]
	public void CanCreateARegisterPlayerCommand()
	{
		var countryGuid = Guid.NewGuid();
		var command = RegisterPlayerCommand.Create("Steve", "Serve", "first.last@tennis.com",
			Gender.Male, new DateTime(2000, 10, 01),
			Plays.LeftHanded, 191, countryGuid);

		command.IsSuccess.Should().BeTrue();
		command.Value.FirstName.Name.Should().Be("Steve");
		command.Value.LastName.Name.Should().Be("Serve");
		command.Value.Email.Email.Should().Be("first.last@tennis.com");
		command.Value.Gender.Should().Be(Gender.Male);
		command.Value.DateOfBirth.Should().Be(new DateTime(2000, 10, 01));
		command.Value.Plays.Should().Be(Plays.LeftHanded);
		command.Value.Height.Value.Should().Be(191);
		command.Value.CountryId.Id.Should().Be(countryGuid);
	}

	[Fact]
	public void WhenCreatingARegisterPlayerCommandWithBadGenderThenErrorResultIsReturned()
	{
		var command = RegisterPlayerCommand.Create("Steve", "Serve", "first.last@tennis.com",
			(Gender)5, new DateTime(2000, 10, 01),
			Plays.LeftHanded, 191, Guid.NewGuid());

		command.IsFailure.Should().BeTrue();
		command.Error.Should().Be("Invalid gender");
	}

	[Fact]
	public void WhenCreatingARegisterPlayerCommandWithBadPlaysThenErrorResultIsReturned()
	{
		var command = RegisterPlayerCommand.Create("Steve", "Serve", "first.last@tennis.com",
			Gender.Female, new DateTime(2000, 10, 01),
			(Plays)3, 191, Guid.NewGuid());

		command.IsFailure.Should().BeTrue();
		command.Error.Should().Be("Invalid plays");
	}
}
