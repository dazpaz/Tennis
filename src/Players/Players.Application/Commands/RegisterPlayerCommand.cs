using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Common;

namespace Players.Application.Commands;

public sealed class RegisterPlayerCommand : ICommand
{
	public string FirstName { get; }
	public string LastName { get; }
	public Gender Gender { get; }
	public DateOnly DateOfBirth { get; }
	public Plays Plays { get; }
	public int Height { get; }
	public string Country { get; }

	private RegisterPlayerCommand(string firstName, string lastName,
		Gender gender, DateOnly dateOfBoth, Plays plays, int height, string country)
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
		Gender gender, DateOnly dateOfBoth, Plays plays, int height, string country)
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
