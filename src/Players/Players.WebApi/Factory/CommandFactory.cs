using CSharpFunctionalExtensions;
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
	}
}
