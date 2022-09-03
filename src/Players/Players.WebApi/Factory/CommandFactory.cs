using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Commands;
using Players.Contract;

namespace Players.WebApi.Factory
{
	public class CommandFactory : ICommandFactory
	{
		public Result<RegisterPlayerCommand> CreateRegisterPlayerCommand(RegisterPlayerDto playerDetails)
		{
			return RegisterPlayerCommand.Create(playerDetails.FirstName, playerDetails.LastName,
				playerDetails.Email, playerDetails.Gender, playerDetails.DateOfBirth, playerDetails.Plays,
				playerDetails.Height, playerDetails.Country);
		}

		public Result<ICommand> CreateUpdateSinglesRankingCommand(Guid id, UpdateRankingDto newRanking)
		{
			return UpdateSinglesRankingCommand.Create(id, newRanking.Rank, newRanking.Points, newRanking.Date);
		}

		public Result<ICommand> CreateUpdateDoublesRankingCommand(Guid id, UpdateRankingDto newRanking)
		{
			return UpdateDoublesRankingCommand.Create(id, newRanking.Rank, newRanking.Points, newRanking.Date);
		}

		public Result<CreateCountryCommand> CreateCreateCountryCommand(CreateCountryDto countryDetails)
		{
			return CreateCountryCommand.Create(countryDetails.ShortName, countryDetails.FullName);
		}
	}
}
