using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
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
	}
}
