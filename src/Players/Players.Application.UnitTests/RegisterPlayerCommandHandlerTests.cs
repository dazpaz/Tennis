using Moq;
using Players.Application.Commands;
using Players.Application.Repository;
using Players.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Application.UnitTests;

public class RegisterPlayerCommandHandlerTests
{
	[Fact]
	public void RegisterPlayerCommandHandlerRegistersPlayerAndSavesToPlayerRepository()
	{
		var command = RegisterPlayerCommand.Create("First", "Last", "first.last@tennis.com",
			Gender.Female, new DateTime(2000, 10, 01), Plays.LeftHanded, 191, Guid.NewGuid());

		Mock<IUnitOfWork> mockUow = new(MockBehavior.Strict);
		mockUow.Setup(u => u.PlayerRepository.Add(It.IsAny<Player>()));
		mockUow.Setup(u => u.SaveChanges()).Returns(1);
		var handler = new RegisterPlayerCommandHandler(mockUow.Object);

		var result = handler.Handle(command.Value);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBe(Guid.Empty);
	}

	[Fact]
	public void RegisterPlayerCommandHandlerReturnFailureResultIfRepositoryFailsToSavePlayer()
	{
		var command = RegisterPlayerCommand.Create("First", "Last", "first.last@tennis.com",
			Gender.Female, new DateTime(2000, 10, 01), Plays.LeftHanded, 191, Guid.NewGuid());

		Mock<IUnitOfWork> mockUow = new(MockBehavior.Strict);
		mockUow.Setup(u => u.PlayerRepository.Add(It.IsAny<Player>()));
		mockUow.Setup(u => u.SaveChanges()).Throws(new Exception("Test Exception Message"));
		var handler = new RegisterPlayerCommandHandler(mockUow.Object);

		var result = handler.Handle(command.Value);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("Test Exception Message");
	}
}
