namespace Players.Contract;

public class CountryDetailsDto
{
	public Guid Id { get; set; }
	public string ShortName { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
}
