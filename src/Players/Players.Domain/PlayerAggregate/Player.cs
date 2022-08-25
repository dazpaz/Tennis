using DomainDesign.Common;
using Players.Common;

#nullable disable

namespace Players.Domain.PlayerAggregate;

public class Player : AggregateRoot<PlayerId>
{
	public PlayerName FirstName { get; private set; }
	public PlayerName LastName { get; private set; }
	public Email Email { get; private set; }
	public Gender Gender { get; private set; }
	public DateTime DateOfBirth { get; private set; }
	public Plays Plays { get; private set; }
	public Height Height { get; private set; }
	public string Country { get; private set; }
	public Ranking SinglesRank { get; private set; }
	public Ranking DoublesRank { get; private set; }
	public RankingPoints SinglesRankingPoints { get; private set; }
	public RankingPoints DoublesRankingPoints { get; private set; }

	protected Player()
	{
	}

	private Player(PlayerId id) : base(id)
	{
	}

	public static Player Register(PlayerName firstName, PlayerName lastName, Email email,
		Gender gender, DateTime dateOfBirth, Plays plays, Height height, string country)
	{
		var initialRank = Ranking.Create(Ranking.MaxRankValue).Value;
		var initialPoints = RankingPoints.Create(RankingPoints.MinPointsValue).Value;

		var player = new Player(new PlayerId())
		{
			FirstName = firstName,
			LastName = lastName,
			Email = email,
			Gender = gender,
			DateOfBirth = dateOfBirth,
			Plays = plays,
			Height = height,
			Country = country,
			SinglesRank = initialRank,
			DoublesRank = initialRank,
			SinglesRankingPoints = initialPoints,
			DoublesRankingPoints = initialPoints
		};

		return player;
	}
}
