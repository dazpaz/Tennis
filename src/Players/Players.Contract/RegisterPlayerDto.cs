using Players.Common;

namespace Players.Contract;

public class RegisterPlayerDto
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateOnly DateOfBirth { get; set; }
	public Plays Plays { get; set; }
	public int Height { get; set; }
	public string Country { get;set; } = string.Empty;
}
