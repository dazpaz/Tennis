using Ardalis.GuardClauses;
using DomainDesign.Common;
using TournamentManagement.Domain.Common;

namespace TournamentManagement.Domain.PlayerAggregate
{
	public class Player : Entity<PlayerId>, IAggregateRoot
	{
		private const ushort MinRank = 1;
		private const ushort MaxRank = 9999;

		public string Name { get; private set; }
		public ushort SinglesRank { get; private set; }
		public ushort DoublesRank { get; private set; }
		public Gender Gender { get; private set; }

		private Player(PlayerId id) : base(id)
		{
		}

		public static Player Register(PlayerId id, string name, ushort singlesRank,
			ushort doublesRank, Gender gender)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));
			GuardAgainstRankOutOfRange(singlesRank, nameof(singlesRank));
			GuardAgainstRankOutOfRange(doublesRank, nameof(doublesRank));

			var player = new Player(id)
			{
				Name = name,
				SinglesRank = singlesRank,
				DoublesRank = doublesRank,
				Gender = gender
			};

			return player;
		}

		public void UpdateRankings(ushort singlesRank, ushort doublesRank)
		{
			SinglesRank = GuardAgainstRankOutOfRange(singlesRank, nameof(singlesRank));
			DoublesRank = GuardAgainstRankOutOfRange(doublesRank, nameof(doublesRank));
		}

		private static ushort GuardAgainstRankOutOfRange(ushort rank, string rankName)
		{
			return Guard.Against.OutOfRange(rank, rankName, MinRank, MaxRank,
				$"{rankName} {rank} is outside allowed range, {MinRank} - {MaxRank}");
		}
	}
}
