using FluentAssertions;
using Xunit;

namespace TournamentManagement.Domain.UnitTests
{
	public class EventGenderValidatorTests
	{
		[Theory]
		[InlineData(EventType.MensSingles, 1, 0)]
		[InlineData(EventType.WomensSingles, 0, 1)]
		[InlineData(EventType.MensDoubles, 2, 0)]
		[InlineData(EventType.WomensDoubles, 0, 2)]
		[InlineData(EventType.MixedDoubles, 1, 1)]
		public void ValidatorReturnsTrueIfCountsMatch(EventType eventType, int maleCount, int femaleCount)
		{
			var result = EventGenderValidator.IsValid(eventType, maleCount, femaleCount);

			result.Should().BeTrue();
		}

		[Theory]
		[InlineData(EventType.MensSingles, 0, 0)]
		[InlineData(EventType.MensSingles, 1, 1)]
		[InlineData(EventType.WomensSingles, 0, 0)]
		[InlineData(EventType.WomensSingles, 1, 1)]
		[InlineData(EventType.MensDoubles, 2, 1)]
		[InlineData(EventType.MensDoubles, 1, 0)]
		[InlineData(EventType.WomensDoubles, 1, 2)]
		[InlineData(EventType.WomensDoubles, 0, 1)]
		[InlineData(EventType.MixedDoubles, 1, 2)]
		[InlineData(EventType.MixedDoubles, 2, 1)]
		public void ValidatorReturnsFalseIfCountsDoNotMatch(EventType eventType, int maleCount, int femaleCount)
		{
			var result = EventGenderValidator.IsValid(eventType, maleCount, femaleCount);

			result.Should().BeFalse();
		}
	}
}
