using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class Player : Entity<Guid>
	{
		private const uint MinRank = 1;
		private const uint MaxRank = 9999;

		public string Name { get; private set; }
		public ushort SinglesRank { get; private set; }
		public ushort DoublesRank { get; private set; }
		public Gender Gender { get; private set; }

		private Player(Guid id) : base(id)
		{
		}

		public static Player Create(string name, ushort singlesRank, ushort doublesRank, Gender gender)
		{
			Guard.ForNullOrEmptyString(name, "Name");
			GuardForRankInRange(singlesRank, "Singles Rank");
			GuardForRankInRange(doublesRank, "Doubles Rank");

			var player = new Player(Guid.NewGuid())
			{
				Name = name,
				SinglesRank = singlesRank,
				DoublesRank = doublesRank,
				Gender = gender
			};

			return player;
		}

		private static void GuardForRankInRange(ushort rank, string rankName)
		{
			if (rank < MinRank || rank > MaxRank)
			{
				throw new ArgumentException($"{rankName} {rank} is outside allowed range, {MinRank} - {MaxRank}");
			}
		}
	}
}
