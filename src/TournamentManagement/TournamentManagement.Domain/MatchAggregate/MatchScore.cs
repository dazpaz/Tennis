using DomainDesign.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TournamentManagement.Contract;
using TournamentManagement.Domain.Common;

namespace TournamentManagement.Domain.MatchAggregate
{
	public class MatchScore : ValueObject<MatchScore>
	{
		public ReadOnlyCollection<int> Sets { get; }
		public ReadOnlyCollection<SetScore> SetScores { get; }
		public Winner Winner { get; } = Winner.Unknown;

		private readonly int[] _sets;
		private readonly IList<SetScore> _setScores;
		private readonly int _setsNeededForVictory;

		public MatchScore(MatchFormat matchFormat, ICollection<SetScore> setScores)
		{
			_sets = new int[] { 0, 0 };
			Sets = new ReadOnlyCollection<int>(_sets);

			_setScores = new List<SetScore>();
			SetScores = new ReadOnlyCollection<SetScore>(_setScores);

			_setsNeededForVictory = GetSetsNeededForVictory(matchFormat.NumberOfSets);

			if (setScores == null || setScores.Count == 0) return;

			GuardAgainstTooManySetScores(matchFormat.NumberOfSets, setScores.Count);

			int setIndex = 0;
			foreach (var setScore in setScores)
			{
				setIndex++;
				_setScores.Add(setScore);

				var setType = setIndex == matchFormat.NumberOfSets
					? matchFormat.FinalSetType
					: SetType.TieBreak;

				var winnerOfTheSet = SetWinnerCalculator.GetWinner(setScore, setType);

				if (winnerOfTheSet == Winner.Unknown) break;

				if (UpdateSetScoreAndCheckForVictory(winnerOfTheSet))
				{
					Winner = winnerOfTheSet;
					break;
				}
			}
		}

		private bool UpdateSetScoreAndCheckForVictory(Winner setWinner)
		{
			var winnerIndex = (int)setWinner;
			_sets[winnerIndex]++;
			return _sets[winnerIndex] == _setsNeededForVictory;
		}

		private static int GetSetsNeededForVictory(int numberOfSets)
		{
			if (numberOfSets == 1) return 1;
			if (numberOfSets == 3) return 2;
			return 3;
		}

		private static void GuardAgainstTooManySetScores(int maxNumberOfSets, int numberOfSets)
		{
			if (numberOfSets > maxNumberOfSets)
			{
				throw new Exception($"Match score has {numberOfSets} sets, but can only have up to {maxNumberOfSets} sets");
			}
		}
	}
}
