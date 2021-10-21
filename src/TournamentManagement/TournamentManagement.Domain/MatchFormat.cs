using DomainDesign.Common;
using System;
using System.Linq;

namespace TournamentManagement.Domain
{
	public class MatchFormat : ValueObject<MatchFormat>
	{
		private static readonly int[] AllowedNumberOfSets = { 1, 3, 5 };

		public static MatchFormat OneSetMatchWithFinalSetTieBreak => new(1, FinalSetType.TieBreak);
		public static MatchFormat ThreeSetMatchWithFinalSetTieBreak => new(3, FinalSetType.TieBreak);
		public static MatchFormat FiveSetMatchWithFinalSetTieBreak => new(5, FinalSetType.TieBreak);
		public static MatchFormat OneSetMatchWithTwoGamesClear => new(1, FinalSetType.TwoGamesClear);
		public static MatchFormat ThreeSetMatchWithTwoGamesClear => new(3, FinalSetType.TwoGamesClear);
		public static MatchFormat FiveSetMatchWithTwoGamesClear => new(5, FinalSetType.TwoGamesClear);

		public int NumberOfSets { get; }
		public FinalSetType FinalSetType { get; }

		public MatchFormat(int numberOfSets, FinalSetType finalSetType)
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
