using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;
using Players.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands;

public sealed class RegisterPlayerCommand : ICommand
{
	public string FirstName { get; }
	public string LastName { get; }
	public Gender Gender { get; }
	public DateTime DateOfBirth { get; }
	public Plays Plays { get; }
	public int Height { get; }
	public string Country { get; }

	private RegisterPlayerCommand(string firstName, string lastName,
		Gender gender, DateTime dateOfBoth, Plays plays, int height, string country)
	{
		FirstName = firstName;
		LastName = lastName;
		Gender = gender;
		DateOfBirth = dateOfBoth;
		Plays = plays;
		Height = height;
		Country = country;
	}

	public static Result<RegisterPlayerCommand> Create(string firstName, string lastName,
		Gender gender, DateTime dateOfBoth, Plays plays, int height, string country)
	{
		try
		{
			if (!Enum.IsDefined(typeof(Gender), gender))
			{
				return Result.Failure<RegisterPlayerCommand>("Invalid gender");
			}

			if (!Enum.IsDefined(typeof(Plays), plays))
			{
				return Result.Failure<RegisterPlayerCommand>("Invalid plays");
			}

			var command = new RegisterPlayerCommand(firstName, lastName, gender, dateOfBoth,
				plays, height, country);

			return Result.Success(command);
		}
		catch (Exception ex)
		{
			return Result.Failure<RegisterPlayerCommand>(ex.Message);
		}
	}
}

public sealed class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand, Guid>
{
	private readonly IUnitOfWork _uow;

	public RegisterPlayerCommandHandler(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public Result<Guid> Handle(RegisterPlayerCommand command)
	{
		try
		{
			var player = Player.Register(command.FirstName, command.LastName, command.Gender,
				command.DateOfBirth, command.Plays, command.Height, command.Country);

			_uow.PlayerRepository.Add(player);
			_uow.SaveChanges();

			return Result.Success(player.Id.Id);
		}
		catch (Exception ex)
		{
			return Result.Failure<Guid>(ex.Message);
		}
	}
}
