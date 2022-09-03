using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Microsoft.AspNetCore.Mvc;
using Players.Application.Commands;
using Players.Common;
using Players.Contract;
using Players.Query;
using Players.WebApi.Controllers;
using Players.WebApi.Factory;

namespace Players.WebApi.UnitTests
{
	public class PlayersControllerTests
	{
		private readonly Mock<IQueryFactory> _mockQueryFactory;
		private readonly Mock<IMessageDispatcher> _mockDispatcher;
		private readonly Mock<ICommandFactory> _mockCommandFactory;

		public PlayersControllerTests()
		{
			_mockQueryFactory = new(MockBehavior.Strict);
			_mockDispatcher = new(MockBehavior.Strict);
			_mockCommandFactory = new(MockBehavior.Strict);
		}

		[Fact]
		public void RegisterPlayerCommand_IfCommandCreationFails_ReturnsBadRequestWithCorrectError()
		{
			var playerDetails = GetTestPlayerDetails();

			_mockCommandFactory.Setup(f => f.CreateRegisterPlayerCommand(playerDetails))
				.Returns(Result.Failure<RegisterPlayerCommand>("Command Creation Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.RegisterPlayer(playerDetails);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Creation Error");

			VerifyAllMocks();
		}

		[Fact]
		public void RegisterPlayerCommand_IfCommandHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var playerDetails = GetTestPlayerDetails();
			var command = GetTestRegisterPlayerCommand();

			_mockCommandFactory.Setup(f => f.CreateRegisterPlayerCommand(playerDetails))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch<Guid>(command.Value))
				.Returns(Result.Failure<Guid>("Command Handler Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.RegisterPlayer(playerDetails);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Handler Error");

			VerifyAllMocks();
		}

		[Fact]
		public void RegisterPlayerCommand_IfCommandHandlerIndicatesSuccess_ReturnCreatedResponse()
		{
			var playerDetails = GetTestPlayerDetails();
			var command = GetTestRegisterPlayerCommand();
			var playerGuid = Guid.NewGuid();

			_mockCommandFactory.Setup(f => f.CreateRegisterPlayerCommand(playerDetails))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch<Guid>(command.Value)).Returns(Result.Success(playerGuid));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (CreatedAtActionResult)controller.RegisterPlayer(playerDetails);

			result.StatusCode.Should().Be(201);
			result.RouteValues!["id"].Should().Be(playerGuid);
		}

		[Fact]
		public void GetPlayer_IfQueryHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var playerGuid = Guid.NewGuid();
			var query = new GetPlayerDetails(playerGuid);
			_mockQueryFactory.Setup(f => f.CreateGetPlayerDetailsQuery(playerGuid)).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Failure<PlayerDetailsDto>("Query Handler Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.GetPlayer(playerGuid);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Query Handler Error");
		}

		[Fact]
		public void GetPlayer_IfQueryHandlerIndicatesSuccess_PlayerDetailsAreReturned()
		{
			var playerGuid = Guid.NewGuid();
			var query = new GetPlayerDetails(playerGuid);
			_mockQueryFactory.Setup(f => f.CreateGetPlayerDetailsQuery(playerGuid)).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Success<PlayerDetailsDto>(new PlayerDetailsDto { FirstName = "Test Name"}));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkObjectResult)controller.GetPlayer(playerGuid);

			result.StatusCode.Should().Be(200);
			var value = result.Value as PlayerDetailsDto;
			value!.FirstName.Should().Be("Test Name");
		}

		[Fact]
		public void GetPlayers_IfQueryHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var query = new GetPlayerSummaryList();
			_mockQueryFactory.Setup(f => f.CreateGetPlayerSummaryListQuery()).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Failure<IList<PlayerSummaryDto>>("Query Handler Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.GetPlayers();

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Query Handler Error");
		}

		[Fact]
		public void GetPlayers_IfQueryHandlerIndicatesSuccess_PlayerSummaryListIsReturned()
		{
			var query = new GetPlayerSummaryList();
			_mockQueryFactory.Setup(f => f.CreateGetPlayerSummaryListQuery()).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Success<IList<PlayerSummaryDto>>(new List<PlayerSummaryDto>
				{
					new PlayerSummaryDto { FullName = "Test Name 1" },
					new PlayerSummaryDto { FullName = "Test Name 2" },
				}));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkObjectResult)controller.GetPlayers();

			result.StatusCode.Should().Be(200);
			var value = result.Value as IList<PlayerSummaryDto>;
			value!.Count.Should().Be(2);
			value![0].FullName.Should().Be("Test Name 1");
			value![1].FullName.Should().Be("Test Name 2");
		}

		[Fact]
		public void UpdateSinglesRanking_IfCommandCreationFails_ReturnsBadRequestWithCorrectError()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();

			_mockCommandFactory.Setup(f => f.CreateUpdateSinglesRankingCommand(id, newRanking))
				.Returns(Result.Failure<ICommand>("Command Creation Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.UpdateSinglesRanking(id, newRanking);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Creation Error");

			VerifyAllMocks();
		}

		[Fact]
		public void UpdateSinglesRanking_IfCommandHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();
			var command = GetTestUpdateSinglesRankCommand();

			_mockCommandFactory.Setup(f => f.CreateUpdateSinglesRankingCommand(id, newRanking))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch(command.Value))
				.Returns(Result.Failure<Guid>("Command Handler Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.UpdateSinglesRanking(id, newRanking);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Handler Error");

			VerifyAllMocks();
		}

		[Fact]
		public void UpdateSinglesRanking_IfCommandHandlerIndicatesSuccess_ReturnCreatedResponse()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();
			var command = GetTestUpdateSinglesRankCommand();

			_mockCommandFactory.Setup(f => f.CreateUpdateSinglesRankingCommand(id, newRanking))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch(command.Value)).Returns(Result.Success());

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkResult)controller.UpdateSinglesRanking(id, newRanking);

			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public void UpdateDoublesRanking_IfCommandCreationFails_ReturnsBadRequestWithCorrectError()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();

			_mockCommandFactory.Setup(f => f.CreateUpdateDoublesRankingCommand(id, newRanking))
				.Returns(Result.Failure<ICommand>("Command Creation Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.UpdateDoublesRanking(id, newRanking);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Creation Error");

			VerifyAllMocks();
		}

		[Fact]
		public void UpdateDoublesRanking_IfCommandHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();

			var command = GetTestUpdateDoublesRankCommand();

			_mockCommandFactory.Setup(f => f.CreateUpdateDoublesRankingCommand(id, newRanking))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch(command.Value))
				.Returns(Result.Failure<Guid>("Command Handler Error"));

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.UpdateDoublesRanking(id, newRanking);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Handler Error");

			VerifyAllMocks();
		}

		[Fact]
		public void UpdateDoublesRanking_IfCommandHandlerIndicatesSuccess_ReturnCreatedResponse()
		{
			var newRanking = GetTestNewRanking();
			var id = Guid.NewGuid();
			var command = GetTestUpdateDoublesRankCommand();

			_mockCommandFactory.Setup(f => f.CreateUpdateDoublesRankingCommand(id, newRanking))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch(command.Value)).Returns(Result.Success());

			var controller = new PlayersController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkResult)controller.UpdateDoublesRanking(id, newRanking);

			result.StatusCode.Should().Be(200);
		}

		private static UpdateRankingDto GetTestNewRanking()
		{
			return new UpdateRankingDto
			{
				Rank = 45,
				Points = 1000,
				Date = new DateTime(2022, 09, 03)
			};
		}

		private static Result<ICommand> GetTestUpdateSinglesRankCommand()
		{
			return UpdateSinglesRankingCommand.Create(Guid.NewGuid(), 40, 1000, new DateTime(2022, 09, 03));
		}

		private static Result<ICommand> GetTestUpdateDoublesRankCommand()
		{
			return UpdateDoublesRankingCommand.Create(Guid.NewGuid(), 40, 1000, new DateTime(2022, 09, 03));
		}

		private void VerifyAllMocks()
		{
			_mockQueryFactory.VerifyAll();
			_mockDispatcher.VerifyAll();
			_mockCommandFactory.VerifyAll();
		}

		private static Result<RegisterPlayerCommand> GetTestRegisterPlayerCommand()
		{
			return RegisterPlayerCommand.Create("First", "Last", "email@company.com", Gender.Female,
				new DateTime(2000, 10, 01), Plays.LeftHanded, 170, Guid.NewGuid());
		}

		private static RegisterPlayerDto GetTestPlayerDetails()
		{
			return new RegisterPlayerDto
			{
				FirstName = "Test",
				LastName = "Player"
			};
		}
	}
}
