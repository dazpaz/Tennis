using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Commands;
using Players.Contract;

namespace Players.WebApi.Factory
{
	public interface ICommandFactory
	{
		Result<RegisterPlayerCommand> CreateRegisterPlayerCommand(RegisterPlayerDto playerDetails);
		Result<ICommand> CreateUpdateSinglesRankingCommand(Guid id, UpdateRankingDto newRanking);
		Result<ICommand> CreateUpdateDoublesRankingCommand(Guid id, UpdateRankingDto newRanking);
	}
}
