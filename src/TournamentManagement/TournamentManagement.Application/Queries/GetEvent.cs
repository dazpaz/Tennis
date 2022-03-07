﻿using DomainDesign.Common;
using System;
using TournamentManagement.Application.Repository;
using TournamentManagement.Common;
using TournamentManagement.Contract;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.Application.Queries
{
	public sealed class GetEvent : IQuery<EventDto>
	{
		public TournamentId Id { get; }
		public EventType EventType { get; }

		public GetEvent(Guid tournamentId, EventType eventType)
		{
			Id = new TournamentId(tournamentId);
			EventType = eventType;
		}
	}

	public sealed class GetEventHandler
		: IQueryHandler<GetEvent, EventDto>
	{
		private readonly IUnitOfWork _uow;

		public GetEventHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public EventDto Handle(GetEvent query)
		{
			var tournament = _uow.TournamentRepository.GetById(query.Id);
			var tennisEvent =  tournament.GetEvent(query.EventType);
			return Convert.ToEventDto(tennisEvent);
		}
	}
}
