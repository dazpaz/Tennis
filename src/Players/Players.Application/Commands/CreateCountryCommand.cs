using CSharpFunctionalExtensions;
using DomainDesign.Common;

namespace Players.Application.Commands;

public sealed class CreateCountryCommand : ICommand
{
	public string ShortName{ get; }
	public string FullName { get; }

	private CreateCountryCommand(string shortName, string fullName)
	{
		ShortName = shortName;
		FullName = fullName;
	}

	public static Result<CreateCountryCommand> Create(string shortName, string fullName)
	{
		// ToDo: In its current form, the creation of the command never throws an exception
		try
		{
			var command = new CreateCountryCommand(shortName, fullName);

			return Result.Success(command);
		}
		catch (Exception ex)
		{
			return Result.Failure<CreateCountryCommand>(ex.Message);
		}
	}
}
