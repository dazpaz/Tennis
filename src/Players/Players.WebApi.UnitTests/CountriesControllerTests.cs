using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Players.Application.Commands;
using Players.Contract;
using Players.WebApi.Controllers;
using Players.WebApi.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Players.WebApi.UnitTests
{
	public class CountriesControllerTests
	{
		private readonly Mock<IQueryFactory> _mockQueryFactory;
		private readonly Mock<IMessageDispatcher> _mockDispatcher;
		private readonly Mock<ICommandFactory> _mockCommandFactory;

		public CountriesControllerTests()
		{
			_mockQueryFactory = new(MockBehavior.Strict);
			_mockDispatcher = new(MockBehavior.Strict);
			_mockCommandFactory = new(MockBehavior.Strict);
		}

		[Fact]
		public void CreateCountryCommand_IfCommandCreationFails_ReturnsBadRequestWithCorrectError()
		{
			var countryDetails = GetTestCountryDetails();

			_mockCommandFactory.Setup(f => f.CreateCreateCountryCommand(countryDetails))
				.Returns(Result.Failure<CreateCountryCommand>("Command Creation Error"));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.CreateCountry(countryDetails);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Creation Error");

			VerifyAllMocks();
		}

		[Fact]
		public void CreateCountryCommand_IfCommandHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var countryDetails = GetTestCountryDetails();
			var command = GetTestCreateCountryCommand();

			_mockCommandFactory.Setup(f => f.CreateCreateCountryCommand(countryDetails))
				.Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch<Guid>(command.Value))
				.Returns(Result.Failure<Guid>("Command Handler Error"));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.CreateCountry(countryDetails);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Command Handler Error");

			VerifyAllMocks();
		}

		[Fact]
		public void CreateCountryCommand_IfCommandHandlerIndicatesSuccess_ReturnCreatedResponse()
		{
			var countryDetails = GetTestCountryDetails();
			var command = GetTestCreateCountryCommand();
			var countryGuid = Guid.NewGuid();

			_mockCommandFactory.Setup(f => f.CreateCreateCountryCommand(countryDetails)).Returns(command);
			_mockDispatcher.Setup(d => d.Dispatch<Guid>(command.Value)).Returns(Result.Success(countryGuid));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (CreatedAtActionResult)controller.CreateCountry(countryDetails);

			result.StatusCode.Should().Be(201);
			result.RouteValues!["id"].Should().Be(countryGuid);
		}

		private static CreateCountryDto GetTestCountryDetails()
		{
			return new CreateCountryDto
			{
				ShortName = "GBR",
				FullName = "Great Britain"
			};
		}

		private static Result<CreateCountryCommand> GetTestCreateCountryCommand()
		{
			return CreateCountryCommand.Create("GBR", "Great Britain");
		}

		private void VerifyAllMocks()
		{
			_mockQueryFactory.VerifyAll();
			_mockDispatcher.VerifyAll();
			_mockCommandFactory.VerifyAll();
		}
	}
}
