namespace Players.Contract;

public class PlayerDetailsDto : PlayerSummaryDto
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateTime DateOfBirth { get; set; }
	public string Country { get; set; } = string.Empty;
	public int SinglesRankingPoints { get; set; }
	public int DoublesRankingPoints { get; set; }
}
