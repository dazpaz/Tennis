using DomainDesign.Common;

#nullable disable

namespace Players.Domain.CountryAggregate;

public class Country : AggregateRoot<CountryId>
{
	public const int MaxShortNameLen = 4;
	public const int MaxFullNameLen = 50;

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
		ValidateParameters(shortName, fullName);

		var country = new Country(new CountryId())
		{
			ShortName = shortName,
			FullName = fullName
		};

		return country;
	}

	public void Update(string shortName, string fullName)
	{
		ValidateParameters(shortName, fullName);

		ShortName = shortName;
		FullName = fullName;
	}

	private static void ValidateParameters(string shortName, string fullName)
	{
		if (string.IsNullOrEmpty(shortName) || shortName.Length > MaxShortNameLen)
		{
			throw new ArgumentException("Invalid short name");
		}

		if (string.IsNullOrEmpty(fullName) || fullName.Length > MaxFullNameLen)
		{
			throw new ArgumentException("Invalid full name");
		}
	}
}
