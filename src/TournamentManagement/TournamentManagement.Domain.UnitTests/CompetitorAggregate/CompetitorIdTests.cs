using FluentAssertions;
using System;
using TournamentManagement.Domain.CompetitorAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.CompetitorAggregate
{
	public class CompetitorIdTests
	{
		[Fact]
		public void CanCreateCompetitorIdWithNoParameters()
		{
			var competitorId = new CompetitorId();

			competitorId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateCompetitorIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var competitorId = new CompetitorId(id);

			competitorId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateCompetitorIdUsingAnEmptyGuid()
		{
			Action act = () => new CompetitorId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}

		[Fact]
		public void ThereIsAByeCompetitorId()
		{
			CompetitorId.Bye.Id.Should().NotBe(Guid.Empty);

			var c1 = CompetitorId.Bye;
			var c2 = CompetitorId.Bye;

			(c1 == c2).Should().BeTrue();
		}
	}
}
