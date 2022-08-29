namespace Players.Contract;

public class PlayerDetailsDto : PlayerSummaryDto
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateTime DateOfBirth { get; set; }
	public int SinglesRankingPoints { get; set; }
	public int DoublesRankingPoints { get; set; }
	public string CountryTitle { get; set;} = string.Empty;
}
