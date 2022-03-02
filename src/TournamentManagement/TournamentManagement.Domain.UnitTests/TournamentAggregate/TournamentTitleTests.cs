using FluentAssertions;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentTitleTests
	{
		[Fact]
		public void CanCreateTournamentTitle()
		{
			var title = new TournamentTitle("Wimbledon");

			title.Title.Should().Be("Wimbledon");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("     ")]
		[InlineData("")]
		public void CannotCreateEmptyTournamentTitle(string title)
		{
			Action act = () => _ = new TournamentTitle(title);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(title == null
					? "Value cannot be null. (Parameter 'title')"
					: "Required input title was empty. (Parameter 'title')");
		}

		[Fact]
		public void CannotCreateTournamentTitleLongerThanMaxLength()
		{
			var tooLong = new string('A', TournamentTitle.MaxLength + 1);

			Action act = () => _ = new TournamentTitle(tooLong);

			act.Should()
				.Throw<ArgumentOutOfRangeException>()
				.WithMessage($"Tournament title was too long, maximum length is {TournamentTitle.MaxLength}");
		}
	}
}
