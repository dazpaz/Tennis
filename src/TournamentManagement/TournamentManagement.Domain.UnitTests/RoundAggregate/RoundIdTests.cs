using FluentAssertions;
using System;
using TournamentManagement.Domain.RoundAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.RoundAggregate
{
	public class RoundIdTests
	{
		[Fact]
		public void CanCreateRoundIdWithNoParameters()
		{
			var roundId = new RoundId();

			roundId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateTournamentIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var roundId = new RoundId(id);

			roundId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateTournamentIdUsingAnEmptyGuid()
		{
			Action act = () => new RoundId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
