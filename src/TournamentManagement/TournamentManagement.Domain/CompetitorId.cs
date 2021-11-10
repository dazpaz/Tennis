using DomainDesign.Common;
using System;

namespace TournamentManagement.Domain
{
	public class CompetitorId : EntityId
	{
		public static CompetitorId Bye => new(new Guid("712917f4-cad8-48c7-a93a-3e8501e7e70e"));

		public CompetitorId() : base() { }
		public CompetitorId(Guid id) : base(id) { }
	}
}
