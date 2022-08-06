using CSharpFunctionalExtensions;
using DomainDesign.Common;

namespace Cqrs.Common.Application;

public sealed class MessageDispatcher
{
	private readonly IServiceProvider _provider;

	public MessageDispatcher(IServiceProvider provider)
	{
		_provider = provider;
	}

	public Result Dispatch(ICommand command)
	{
		Type type = typeof(ICommandHandler<>);
		Type[] typeArgs = { command.GetType() };
		Type handlerType = type.MakeGenericType(typeArgs);

		dynamic? handler = _provider.GetService(handlerType);

		return handler == null
			? Result.Failure($"No handler defined for {command.GetType().Name} command")
			: handler.Handle((dynamic)command);
	}

	public Result<TResult> Dispatch<TResult>(ICommand command)
	{
		Type type = typeof(ICommandHandler<,>);
		Type[] typeArgs = { command.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(typeArgs);

		dynamic? handler = _provider.GetService(handlerType);

		return handler == null
			? Result.Failure<TResult>($"No handler defined for {command.GetType().Name} command")
			: handler.Handle((dynamic)command);
	}

	public Result<TResult> Dispatch<TResult>(IQuery<TResult> query)
	{
		Type type = typeof(IQueryHandler<,>);
		Type[] typeArgs = { query.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(typeArgs);

		dynamic? handler = _provider.GetService(handlerType);

		return handler == null
			? Result.Failure<TResult>($"No handler defined for {query.GetType().Name} query")
			: handler.Handle((dynamic)query);
	}
}
