using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using DomainDesign.Common;
using FluentAssertions;
using Moq;

namespace Cqrs.Common.UnitTests.Application;

public sealed class TestCommandType : ICommand
{
}

public sealed class TestQueryType : IQuery<int>
{
}

	public class MessageDispatcherTests
{
	[Fact]
	public void WhenDispatchingCommand_IfProviderCannotFindHandler_FailureResultIsReturned()
	{
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType>))).Returns(null);
		var command = new TestCommandType();
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch(command);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("No handler defined for TestCommandType command");
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingCommand_IfCommandHandlerReturnsSuccess_SuccessResultIsReturned()
	{
		var command = new TestCommandType();
		Mock<ICommandHandler<TestCommandType>> mockCommandHandler = new(MockBehavior.Strict);
		mockCommandHandler.Setup(h => h.Handle(command)).Returns(Result.Success);
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType>))).Returns(mockCommandHandler.Object);
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch(command);

		result.IsSuccess.Should().BeTrue();
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingCommand_IfCommandHandlerReturnsFailure_FailureResultIsReturned()
	{
		var command = new TestCommandType();
		Mock<ICommandHandler<TestCommandType>> mockCommandHandler = new(MockBehavior.Strict);
		mockCommandHandler.Setup(h => h.Handle(command)).Returns(Result.Failure("handler errored"));
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType>))).Returns(mockCommandHandler.Object);
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch(command);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("handler errored");
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingCommandWithReturnValue_IfProviderCannotFindHandler_FailureResultIsReturned()
	{
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType, int>))).Returns(null);
		var command = new TestCommandType();
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch<int>(command);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("No handler defined for TestCommandType command");
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingCommandWithReturnValue_IfCommandHandlerReturnsSuccess_SuccessResultIsReturned()
	{
		var command = new TestCommandType();
		Mock<ICommandHandler<TestCommandType,int>> mockCommandHandler = new(MockBehavior.Strict);
		mockCommandHandler.Setup(h => h.Handle(command)).Returns(Result.Success(42));
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType, int>))).Returns(mockCommandHandler.Object);
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch<int>(command);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Be(42);
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingCommandWithReturnValue_IfCommandHandlerReturnsFailure_FailureResultIsReturned()
	{
		var command = new TestCommandType();
		Mock<ICommandHandler<TestCommandType, int>> mockCommandHandler = new(MockBehavior.Strict);
		mockCommandHandler.Setup(h => h.Handle(command)).Returns(Result.Failure<int>("handler errored"));
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(ICommandHandler<TestCommandType, int>))).Returns(mockCommandHandler.Object);
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch<int>(command);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("handler errored");
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingQuery_IfProviderCannotFindHandler_FailureResultIsReturned()
	{
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(IQueryHandler<TestQueryType, int>))).Returns(null);
		var query = new TestQueryType();
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch(query);

		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be("No handler defined for TestQueryType query");
		Mock.VerifyAll();
	}

	[Fact]
	public void WhenDispatchingQuery_QueryHandlerIsCalled_AndQueryResultIsReturned()
	{
		var query = new TestQueryType();
		Mock<IQueryHandler<TestQueryType, int>> mockQueryHandler = new(MockBehavior.Strict);
		mockQueryHandler.Setup(h => h.Handle(query)).Returns(42);
		Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);
		mockServiceProvider.Setup(x => x.GetService(typeof(IQueryHandler<TestQueryType, int>)))
			.Returns(mockQueryHandler.Object);
		var dispatcher = new MessageDispatcher(mockServiceProvider.Object);

		var result = dispatcher.Dispatch<int>(query);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Be(42);
		Mock.VerifyAll();
	}
}
