using DomainDesign.Common;
using System;
using TournamentManagement.Domain.Common;

namespace TournamentManagement.Domain.PlayerAggregate
{
	public class Player : Entity<PlayerId>, IAggregateRoot
	{
		private const uint MinRank = 1;
		private const uint MaxRank = 9999;

		public string Name { get; private set; }
		public ushort SinglesRank { get; private set; }
		public ushort DoublesRank { get; private set; }
		public Gender Gender { get; private set; }

		private Player(PlayerId id) : base(id)
		{
		}

		public static Player Create(PlayerId id, string name, ushort singlesRank,
			ushort doublesRank, Gender gender)
		{
			Guard.AgainstNullOrEmptyString(name, nameof(name));
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

		private static void GuardAgainstRankOutOfRange(ushort rank, string rankName)
		{
			if (rank < MinRank || rank > MaxRank)
			{
				throw new ArgumentException($"{rankName} {rank} is outside allowed range, {MinRank} - {MaxRank}");
			}
		}
	}
}
