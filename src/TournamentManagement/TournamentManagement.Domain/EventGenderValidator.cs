using System.Collections.Generic;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Domain
{
	public static class EventGenderValidator
	{
		class GenderCounts
		{
			public int MaleCount { get; set; }
			public int FemaleCount { get; set; }
		}

		private static readonly IDictionary<EventType, GenderCounts> ValidCounts =
			new Dictionary<EventType, GenderCounts>
		{
			{ EventType.MensSingles, new GenderCounts { MaleCount = 1 , FemaleCount = 0 } },
			{ EventType.WomensSingles, new GenderCounts { MaleCount = 0 , FemaleCount = 1 } },
			{ EventType.MensDoubles , new GenderCounts { MaleCount = 2 , FemaleCount = 0 } },
			{ EventType.WomensDoubles, new GenderCounts { MaleCount = 0 , FemaleCount = 2 } },
			{ EventType.MixedDoubles, new GenderCounts { MaleCount = 1 , FemaleCount = 1 } }
		};

		public static bool IsValid(EventType eventType, int maleCount, int femaleCount)
		{
			var expected = ValidCounts[eventType];
			return expected.MaleCount == maleCount && expected.FemaleCount == femaleCount;
		}
	}
}
