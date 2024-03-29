﻿using Ardalis.GuardClauses;
using DomainDesign.Common;
using TournamentManagement.Common;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Domain.CompetitorAggregate
{
	public class Competitor : AggregateRoot<CompetitorId>
	{
		public virtual Tournament Tournament { get; private set; }
		public EventType EventType { get; private set; }
		public Seeding Seeding { get; private set; }
		public string PlayerOneName { get; private set; }
		public string PlayerTwoName { get; private set; }

		protected Competitor()
		{
		}

		private Competitor(CompetitorId id) : base(id)
		{
		}

		public static Competitor Create(Tournament tournament, EventType eventType,
			Seeding seeding, string playerOneName, string playerTwoName = null)
		{
			Guard.Against.Null(tournament, nameof(tournament));
			Guard.Against.NullOrWhiteSpace(playerOneName, nameof(playerOneName));
			if (!Event.IsSinglesEvent(eventType))
			{
				Guard.Against.NullOrWhiteSpace(playerTwoName, nameof(playerTwoName));
			}

			var competitor = new Competitor(new CompetitorId())
			{
				Tournament = tournament,
				EventType = eventType,
				Seeding = seeding,
				PlayerOneName = playerOneName,
				PlayerTwoName = playerTwoName
			};

			return competitor;
		}
	}
}
