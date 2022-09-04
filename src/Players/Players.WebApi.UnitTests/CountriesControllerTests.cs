using Cqrs.Common.Application;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Players.Application.Commands;
using Players.Contract;
using Players.Query;
using Players.WebApi.Controllers;
using Players.WebApi.Factory;

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

		[Fact]
		public void GetCountry_IfQueryHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var countryGuid = Guid.NewGuid();
			var query = new GetCountryDetails(countryGuid);
			_mockQueryFactory.Setup(f => f.CreateGetCountryDetailsQuery(countryGuid)).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Failure<CountryDetailsDto>("Query Handler Error"));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.GetCountry(countryGuid);

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Query Handler Error");
		}

		[Fact]
		public void GetCountry_IfQueryHandlerIndicatesSuccess_CountryDetailsAreReturned()
		{
			var countryGuid = Guid.NewGuid();
			var query = new GetCountryDetails(countryGuid);
			_mockQueryFactory.Setup(f => f.CreateGetCountryDetailsQuery(countryGuid)).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Success(new CountryDetailsDto
				{ 
					ShortName = "GBR",
					FullName = "Great Britain"
				}));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkObjectResult)controller.GetCountry(countryGuid);

			result.StatusCode.Should().Be(200);
			var value = result.Value as CountryDetailsDto;
			value!.ShortName.Should().Be("GBR");
			value!.FullName.Should().Be("Great Britain");
		}

		[Fact]
		public void GetCountries_IfQueryHandlerHasAnError_ReturnsBadRequestWithCorrectError()
		{
			var query = new GetCountryDetailsList();
			_mockQueryFactory.Setup(f => f.CreateGetCountryDetailsListQuery()).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Failure<IList<CountryDetailsDto>>("Query Handler Error"));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (BadRequestObjectResult)controller.GetCountries();

			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Query Handler Error");
		}

		[Fact]
		public void GetCountries_IfQueryHandlerIndicatesSuccess_CountriesListIsReturned()
		{
			var query = new GetCountryDetailsList();
			_mockQueryFactory.Setup(f => f.CreateGetCountryDetailsListQuery()).Returns(query);

			_mockDispatcher.Setup(d => d.Dispatch(query))
				.Returns(Result.Success<IList<CountryDetailsDto>>(new List<CountryDetailsDto>
				{
					new CountryDetailsDto { ShortName = "GBR", FullName = "Great Britain" },
					new CountryDetailsDto { ShortName = "FRA", FullName = "France" },
				}));

			var controller = new CountriesController(_mockDispatcher.Object,
				_mockCommandFactory.Object, _mockQueryFactory.Object);

			var result = (OkObjectResult)controller.GetCountries();

			result.StatusCode.Should().Be(200);
			var value = result.Value as IList<CountryDetailsDto>;
			value!.Count.Should().Be(2);
			value![0].ShortName.Should().Be("GBR");
			value![1].FullName.Should().Be("France");
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
