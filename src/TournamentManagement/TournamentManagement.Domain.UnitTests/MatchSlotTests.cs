using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class MatchSlotTests
	{
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void CanCreateMatchSlot(int slot)
		{
			var matchId = new MatchId();
			var matchSlot = new MatchSlot(matchId, slot);

			matchSlot.MatchId.Should().Be(matchId);
			matchSlot.Slot.Should().Be(slot);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		public void CannotCreateMatchSlotIfTheSlotIsInvalid(int slot)
		{
			var matchId = new MatchId();

			Action act = () => new MatchSlot(matchId, slot);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("ss");
		}
	}
}
