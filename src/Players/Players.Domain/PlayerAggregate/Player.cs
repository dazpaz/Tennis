using DomainDesign.Common;
using Players.Common;

#nullable disable

namespace Players.Domain.PlayerAggregate;

public class Player : AggregateRoot<PlayerId>
{
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string FullName { get; private set; }
	public Gender Gender { get; private set; }
	public DateTime DateOfBirth { get; private set; }
	public Plays Plays { get; private set; }
	public int Height { get; private set; }
	public string Country { get; private set; }
	public int SinglesRank { get; private set; }
	public int DoublesRank { get; private set; }
	public int SinglesRankingPoints { get; private set; }
	public int DoublesRankingPoints { get; private set; }

	protected Player()
	{
	}

	private Player(PlayerId id) : base(id)
	{
	}

	public static Player Register(string firstName, string lastName, Gender gender,
		DateTime dateOfBirth, Plays plays, int height, string country)
	{
		var player = new Player(new PlayerId())
		{
			FirstName = firstName,
			LastName = lastName,
			FullName = $"{firstName} {lastName}",
			Gender = gender,
			DateOfBirth = dateOfBirth,
			Plays = plays,
			Height = height,
			Country = country,
			SinglesRank = 999,
			DoublesRank = 999,
			SinglesRankingPoints = 0,
			DoublesRankingPoints = 0
		};

		return player;
	}
}
