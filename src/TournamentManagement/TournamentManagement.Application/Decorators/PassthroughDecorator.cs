using CSharpFunctionalExtensions;
using DomainDesign.Common;

namespace TournamentManagement.Application.Decorators
{
	public sealed class PassthroughDecorator<TCommand> : ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand> _handler;

		public PassthroughDecorator(ICommandHandler<TCommand> handler)
		{
			_handler = handler;
		}

		public Result Handle(TCommand command)
		{
			var result = _handler.Handle(command);
			return result;
		}
	}

	public sealed class PassthroughDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand, TResult> _handler;

		public PassthroughDecorator(ICommandHandler<TCommand, TResult> handler)
		{
			_handler = handler;
		}

		public Result<TResult> Handle(TCommand command)
		{
			var result = _handler.Handle(command);
			return result;
		}
	}
}
