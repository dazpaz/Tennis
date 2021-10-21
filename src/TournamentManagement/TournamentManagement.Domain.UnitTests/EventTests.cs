using FluentAssertions;
using System;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class EventTests
	{
		[Fact]
		public void CanCreateAnEventUsingTheFactory()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			tennisEvent.Id.Should().NotBe(Guid.Empty);
			tennisEvent.EventType.Should().Be(EventType.MensSingles);
			tennisEvent.MatchFormat.Should().Be(MatchFormat.ThreeSetMatchWithFinalSetTieBreak);
			tennisEvent.EventSize.EntrantsLimit.Should().Be(128);
			tennisEvent.EventSize.NumberOfSeeds.Should().Be(32);
		}

		[Theory]
		[InlineData(EventType.MensDoubles, false)]
		[InlineData(EventType.MensSingles, true)]
		[InlineData(EventType.MixedDoubles, false)]
		[InlineData(EventType.WomensDoubles, false)]
		[InlineData(EventType.WomensSingles, true)]
		public void TheIsSinglesEventPropertyIsSetCorrectlyBasedOnEventType(EventType eventType, bool isSingles)
		{
			var tennisEvent = Event.Create(eventType, 128, 32,
				new MatchFormat(1, FinalSetType.TieBreak));

			tennisEvent.IsSinglesEvent.Should().Be(isSingles);
		}

		[Fact]
		public void CanUpdateAnEvent()
		{
			var tennisEvent = Event.Create(EventType.MensSingles, 128, 32,
				MatchFormat.ThreeSetMatchWithFinalSetTieBreak);

			var id = tennisEvent.Id;

			tennisEvent.UpdateDetails(EventType.MensDoubles, 64, 16,
				MatchFormat.OneSetMatchWithFinalSetTieBreak);

			tennisEvent.Id.Should().Be(id);
			tennisEvent.EventType.Should().Be(EventType.MensDoubles);
			tennisEvent.MatchFormat.Should().Be(MatchFormat.OneSetMatchWithFinalSetTieBreak);
			tennisEvent.EventSize.EntrantsLimit.Should().Be(64);
			tennisEvent.EventSize.NumberOfSeeds.Should().Be(16);
		}
	}
}
