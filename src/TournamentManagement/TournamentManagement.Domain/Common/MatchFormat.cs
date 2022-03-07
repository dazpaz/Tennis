using DomainDesign.Common;
using System;
using System.Linq;
using TournamentManagement.Common;

namespace TournamentManagement.Domain.Common
{
	public class MatchFormat : ValueObject<MatchFormat>
	{
		private static readonly int[] AllowedNumberOfSets = { 1, 3, 5 };

		public static MatchFormat OneSetMatchWithFinalSetTieBreak => new(1, SetType.TieBreak);
		public static MatchFormat ThreeSetMatchWithFinalSetTieBreak => new(3, SetType.TieBreak);
		public static MatchFormat FiveSetMatchWithFinalSetTieBreak => new(5, SetType.TieBreak);
		public static MatchFormat OneSetMatchWithTwoGamesClear => new(1, SetType.TwoGamesClear);
		public static MatchFormat ThreeSetMatchWithTwoGamesClear => new(3, SetType.TwoGamesClear);
		public static MatchFormat FiveSetMatchWithTwoGamesClear => new(5, SetType.TwoGamesClear);

		public int NumberOfSets { get; }
		public SetType FinalSetType { get; }

		public MatchFormat(int numberOfSets, SetType finalSetType)
		{
			ValidateParameters(numberOfSets);

			NumberOfSets = numberOfSets;
			FinalSetType = finalSetType;
		}

		private static void ValidateParameters(int numberOfSets)
		{
			if (!AllowedNumberOfSets.Contains(numberOfSets))
			{
				throw new ArgumentException($"Invalid number of sets, {numberOfSets}.");
			}
		}
	}
}
