using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Common;
using Players.Domain.CountryAggregate;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands;

public sealed class RegisterPlayerCommand : ICommand
{
	public PlayerName FirstName { get; }
	public PlayerName LastName { get; }
	public EmailAddress Email { get; }
	public Gender Gender { get; }
	public DateTime DateOfBirth { get; }
	public Plays Plays { get; }
	public Height Height { get; }
	public CountryId CountryId { get; }
	 
	private RegisterPlayerCommand(PlayerName firstName, PlayerName lastName, EmailAddress email,
		Gender gender, DateTime dateOfBoth, Plays plays, Height height, CountryId countryId)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		Gender = gender;
		DateOfBirth = dateOfBoth;
		Plays = plays;
		Height = height;
		CountryId = countryId;
	}

	public static Result<RegisterPlayerCommand> Create(string firstName, string lastName, string email,
		Gender gender, DateTime dateOfBoth, Plays plays, int height, Guid countryGuid)
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

			var playerEmail = EmailAddress.Create(email);
			if (playerEmail.IsFailure) return Result.Failure<RegisterPlayerCommand>(playerEmail.Error);

			var command = new RegisterPlayerCommand(playerFirstName.Value, playerLastName.Value, 
				playerEmail.Value, gender,
				dateOfBoth, plays, playerHeight.Value, new CountryId(countryGuid));

			return Result.Success(command);
		}
		catch (Exception ex)
		{
			return Result.Failure<RegisterPlayerCommand>(ex.Message);
		}
	}
}
