using CSharpFunctionalExtensions;

namespace DomainDesign.Common
{
	public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
	{
		Result<TResult> Handle(TCommand command);
	}
}
