using DomainDesign.Common;

namespace TournamentManagement.Domain
{
	public class Match : Entity<MatchId>
	{
		public MatchFormat Format { get; private set; }
		public MatchState State { get; private set; }

		private Match(MatchId id) : base(id)
		{
		}

		public static Match Create(MatchFormat format)
		{
			var match = new Match(new MatchId())
			{
				Format = format,
				State = MatchState.Created
			};

			return match;
		}
	}
}
