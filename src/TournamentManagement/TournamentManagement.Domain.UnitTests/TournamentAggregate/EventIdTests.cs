using FluentAssertions;
using System;
using TournamentManagement.Domain.TournamentAggregate;
using Xunit;

namespace TournamentManagement.Domain.UnitTests.TournamentAggregate
{
	public class EventIdTests
	{
		[Fact]
		public void CanCreateEventIdWithNoParameters()
		{
			var eventId = new EventId();

			eventId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateEventIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var eventId = new EventId(id);

			eventId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateEventIdUsingAnEmptyGuid()
		{
			Action act = () => new EventId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}
	}
}
