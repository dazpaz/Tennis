using DomainDesign.Common;

#nullable disable

namespace Players.Domain.CountryAggregate;

public class Country : AggregateRoot<CountryId>
{
	public string ShortName { get; private set; }

	public string FullName { get; private set; }

	protected Country()
	{
	}

	private Country(CountryId id) : base(id)
	{
	}

	public static Country Create(string shortName, string fullName)
	{
		var country = new Country(new CountryId())
		{
			ShortName = shortName,
			FullName = fullName
		};

		return country;
	}

	public void Update(string shortName, string fullName)
	{
		ShortName = shortName;
		FullName = fullName;
	}
}
