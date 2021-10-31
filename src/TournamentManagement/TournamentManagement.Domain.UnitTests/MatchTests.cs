using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class MatchTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateMatchAndItIsCreatedCorrectly()
		{
			var match = Match.Create(MatchFormat.OneSetMatchWithTwoGamesClear);

			match.Id.Id.Should().NotBe(Guid.Empty);
			match.Format.Should().Be(MatchFormat.OneSetMatchWithTwoGamesClear);
			match.State.Should().Be(MatchState.Created);
		}
	}
}
