using CSharpFunctionalExtensions;
using DomainDesign.Common;
using System;
using System.Text.Json;

namespace TournamentManagement.Application.Decorators
{
	public sealed class AuditDecorator<TCommand> : ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand> _handler;

		public AuditDecorator(ICommandHandler<TCommand> handler)
		{
			_handler = handler;
		}

		public Result Handle(TCommand command)
		{
			// Use proper logger, not console writeline

			string commandJson = JsonSerializer.Serialize(command);
			Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

			var result = _handler.Handle(command);

			Console.WriteLine($"Command result for {command.GetType().Name}: {result}");
			return result;
		}
	}

	public sealed class AuditDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
		where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand, TResult> _handler;

		public AuditDecorator(ICommandHandler<TCommand, TResult> handler)
		{
			_handler = handler;
		}

		public Result<TResult> Handle(TCommand command)
		{
			// Use proper logger, not console writeline

			string commandJson = JsonSerializer.Serialize(command);
			Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

			var result = _handler.Handle(command);

			Console.WriteLine($"Command result for {command.GetType().Name}: {result}");
			return result;
		}
	}
}
