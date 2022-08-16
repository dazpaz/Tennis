using CSharpFunctionalExtensions;
using DomainDesign.Common;

namespace Cqrs.Common.Application;

public interface IMessageDispatcher
{
	Result Dispatch(ICommand command);
	Result<TResult> Dispatch<TResult>(ICommand command);
	Result<TResult> Dispatch<TResult>(IQuery<TResult> query);
}
