using Players.Common;

namespace Players.Contract;

public class PlayerSummaryDto
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public Gender Gender { get; set; }
	public Plays Plays { get; set; }
	public int Height { get; set; }
	public int SinglesRank { get; set; }
	public int DoublesRank { get; set; }
	public string CountryCode { get; set; } = string.Empty;
}
