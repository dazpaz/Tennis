using CSharpFunctionalExtensions;
using DomainDesign.Common;

namespace TournamentManagement.Application.Decorators
{
	public sealed class NullDecorator<TCommand> : ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand> _handler;

		public NullDecorator(ICommandHandler<TCommand> handler)
		{
			_handler = handler;
		}

		public Result Handle(TCommand command)
		{
			// Null operation for now - just checking the decorator is being used
			var result = _handler.Handle(command);
			return result;
		}
	}

	public sealed class NullDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand, TResult> _handler;

		public NullDecorator(ICommandHandler<TCommand, TResult> handler)
		{
			_handler = handler;
		}

		public Result<TResult> Handle(TCommand command)
		{
			// Null operation for now - just checking the decorator is being used
			var result = _handler.Handle(command);
			return result;
		}
	}
}
