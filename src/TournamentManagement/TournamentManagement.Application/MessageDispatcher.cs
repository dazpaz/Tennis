using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;

namespace TournamentManagement.Application
{
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

			dynamic handler = _provider.GetService(handlerType);
			Result result = handler.Handle((dynamic)command);

			return result;
		}

		public Result<TResult> Dispatch<TResult>(ICommand command)
		{
			Type type = typeof(ICommandHandler<,>);
			Type[] typeArgs = { command.GetType(), typeof(TResult) };
			Type handlerType = type.MakeGenericType(typeArgs);

			dynamic handler = _provider.GetService(handlerType);
			Result<TResult> result = handler.Handle((dynamic)command);

			return result;
		}

		public Result<TResult> Dispatch<TResult>(IQuery<TResult> query)
		{
			Type type = typeof(IQueryHandler<,>);
			Type[] typeArgs = { query.GetType(), typeof(TResult) };
			Type handlerType = type.MakeGenericType(typeArgs);

			dynamic handler = _provider.GetService(handlerType);
			Result<TResult> result = handler.Handle((dynamic)query);

			return result;
		}
	}
}
