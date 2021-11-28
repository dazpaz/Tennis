using FluentAssertions;
using System;
using TournamentManagement.Domain.MatchAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.MatchAggregate
{
	public class MatchIdTests
	{
		[Fact]
		public void CanCreateMatchIdWithNoParameters()
		{
			var matchId = new MatchId();

			matchId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateMatchIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var matchId = new MatchId(id);

			matchId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateMatchIdUsingAnEmptyGuid()
		{
			Action act = () => new MatchId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
