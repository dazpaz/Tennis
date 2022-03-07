using TournamentManagement.Common;

namespace TournamentManagement.Domain.MatchAggregate
{
	public class SetWinnerCalculator
	{
		public static Winner GetWinner(SetScore setScore, SetType setType)
		{
			if (setType == SetType.TieBreak) return GetTieBreakWinner(setScore, 6, 6);
			if (setType == SetType.TieBreakAtTwelveAll) return GetTieBreakWinner(setScore, 6, 12);

			if (setType == SetType.TwoGamesClear) return GetTwoClearWinner(setScore, 6);
			if (setType == SetType.ChampionsTieBreak) return GetTwoClearWinner(setScore, 10);

			return Winner.Unknown;
		}

		private static Winner GetTieBreakWinner(SetScore setScore, int minScoreToWin, int tieBreakAt)
		{
			var winner = Winner.Unknown;

			// Player wins by 2 clear games
			if (setScore.GamesOne > setScore.GamesTwo + 1 && setScore.GamesOne >= minScoreToWin) return Winner.Competitor1;
			if (setScore.GamesTwo > setScore.GamesOne + 1 && setScore.GamesTwo >= minScoreToWin) return Winner.Competitor2;

			// Player wins in tie break
			if (setScore.GamesOne == tieBreakAt + 1 && setScore.GamesTwo == tieBreakAt) return Winner.Competitor1;
			if (setScore.GamesTwo == tieBreakAt + 1 && setScore.GamesOne == tieBreakAt) return Winner.Competitor2;

			return winner;
		}

		private static Winner GetTwoClearWinner(SetScore setScore, int minScoreToWin)
		{
			var winner = Winner.Unknown;

			if (setScore.GamesOne > setScore.GamesTwo + 1 && setScore.GamesOne >= minScoreToWin) return Winner.Competitor1;
			if (setScore.GamesTwo > setScore.GamesOne + 1 && setScore.GamesTwo >= minScoreToWin) return Winner.Competitor2;

			return winner;
		}
	}
}
