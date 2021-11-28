using FluentAssertions;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentIdTests
	{
		[Fact]
		public void CanCreateTournamentIdWithNoParameters()
		{
			var tournamentId = new TournamentId();

			tournamentId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateTournamentIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var tournamentId = new TournamentId(id);

			tournamentId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateTournamentIdUsingAnEmptyGuid()
		{
			Action act = () => new TournamentId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
