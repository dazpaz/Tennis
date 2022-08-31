using CSharpFunctionalExtensions;
using Players.Application.Commands;
using Players.Contract;

namespace Players.WebApi.Factory
{
	public interface ICommandFactory
	{
		Result<RegisterPlayerCommand> CreateRegisterPlayerCommand(RegisterPlayerDto playerDetails);
	}
}
