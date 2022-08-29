using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;
using Players.Domain.PlayerAggregate;

namespace Players.Application.Commands;

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
			var country = _uow.CountryRepository.GetById(command.CountryId);
			if (country == null) return Result.Failure<Guid>("No such country");

			var player = Player.Register(command.FirstName, command.LastName, command.Email,
				command.Gender, command.DateOfBirth, command.Plays, command.Height,
				country);

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
