using Players.Common;

namespace Players.Contract;

public class RegisterPlayerDto
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public Gender Gender { get; set; }
	public DateTime DateOfBirth { get; set; }
	public Plays Plays { get; set; }
	public int Height { get; set; }
	public Guid Country { get;set; }
}
