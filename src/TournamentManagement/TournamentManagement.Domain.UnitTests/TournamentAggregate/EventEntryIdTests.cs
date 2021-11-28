using FluentAssertions;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class EventEntryIdTests
	{
		[Fact]
		public void CanCreateEventEntryIdWithNoParameters()
		{
			var eventEntryId = new EventEntryId();

			eventEntryId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateEventEntryIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var eventEntryId = new EventEntryId(id);

			eventEntryId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateEventEntryIdUsingAnEmptyGuid()
		{
			Action act = () => new EventEntryId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
