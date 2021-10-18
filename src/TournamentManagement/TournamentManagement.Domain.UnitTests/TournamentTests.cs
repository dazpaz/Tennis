using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class TournamentTests
	{
		[Fact]
		public void CanUseFactoryMethodToCreateTournamentAndItIsCreatedCorrectly()
		{
			var tournament = Tournament.Create("Wimbledon", new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			tournament.Id.Should().NotBe(Guid.Empty);
			tournament.Title.Should().Be("Wimbledon");
			tournament.Year.Should().Be(2019);
			tournament.State.Should().Be(TournamentState.BeingDefined);
			tournament.StartDate.Should().Be(new DateTime(2019, 7, 1));
			tournament.EndDate.Should().Be(new DateTime(2019, 7, 14));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void CannotCreateTournamentWithEmptyTitle(string title)
		{
			Action act = () => Tournament.Create(title, new DateTime(2019, 7, 1), new DateTime(2019, 7, 14));

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Value can not be null or empty string (Parameter 'title')");
		}
	}
}
