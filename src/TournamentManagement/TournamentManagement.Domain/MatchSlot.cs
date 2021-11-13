using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class MatchSlot : ValueObject<MatchSlot>
	{
		public MatchId MatchId { get; }
		public int Slot { get; }

		public MatchSlot(MatchId matchId, int slot)
		{
			GuardAgainstInvalidCompetitorSlot(slot);

			MatchId = matchId;
			Slot = slot;
		}

		private static void GuardAgainstInvalidCompetitorSlot(int slot)
		{
			if (slot < 1 || slot > 2)
			{
				throw new ArgumentException($"Invalid Slot {slot}, it must be 1 or 2");
			}
		}
	}
}
