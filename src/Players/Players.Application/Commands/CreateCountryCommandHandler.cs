using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;
using Players.Domain.CountryAggregate;

namespace Players.Application.Commands;

public sealed class CreateCountryCommandHandler : ICommandHandler<CreateCountryCommand, Guid>
{
	private readonly IUnitOfWork _uow;

	public CreateCountryCommandHandler(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public Result<Guid> Handle(CreateCountryCommand command)
	{
		try
		{
			var country = Country.Create(command.ShortName, command.FullName);

			_uow.CountryRepository.Add(country);
			_uow.SaveChanges();

			// ToDo: Chould we return a Country ID instead of a Guid, and conver higher up
			return Result.Success(country.Id.Id);
		}
		catch (Exception ex)
		{
			return Result.Failure<Guid>(ex.Message);
		}
	}
}
