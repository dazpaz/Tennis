using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;
using Players.Common;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands;

public sealed class RegisterPlayerCommand : ICommand
{
	public PlayerName FirstName { get; }
	public PlayerName LastName { get; }
	public Email Email { get; }
	public Gender Gender { get; }
	public DateTime DateOfBirth { get; }
	public Plays Plays { get; }
	public Height Height { get; }
	public string Country { get; }
	 
	private RegisterPlayerCommand(PlayerName firstName, PlayerName lastName, Email email,
		Gender gender, DateTime dateOfBoth, Plays plays, Height height, string country)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		Gender = gender;
		DateOfBirth = dateOfBoth;
		Plays = plays;
		Height = height;
		Country = country;
	}

	public static Result<RegisterPlayerCommand> Create(string firstName, string lastName, string email,
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

			var playerHeight = Height.Create(height);
			if (playerHeight.IsFailure) return Result.Failure<RegisterPlayerCommand>(playerHeight.Error);

			var playerFirstName = PlayerName.Create(firstName);
			if (playerFirstName.IsFailure) return Result.Failure<RegisterPlayerCommand>(playerFirstName.Error);

			var playerLastName = PlayerName.Create(lastName);
			if (playerLastName.IsFailure) return Result.Failure<RegisterPlayerCommand>(playerLastName.Error);

			var playerEmail = Email.Create(email);
			if (playerEmail.IsFailure) return Result.Failure<RegisterPlayerCommand>(playerEmail.Error);

			var command = new RegisterPlayerCommand(playerFirstName.Value, playerLastName.Value, 
				playerEmail.Value, gender,
				dateOfBoth, plays, playerHeight.Value, country);

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
			var player = Player.Register(command.FirstName, command.LastName, command.Email,
				command.Gender, command.DateOfBirth, command.Plays, command.Height,
				command.Country);

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
