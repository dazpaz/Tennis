using TournamentManagement.Application.Repository;

namespace TournamentManagement.Data.Repository
{
	class UnitOfWork : IUnitOfWork
	{
		private readonly TournamentManagementDbContext _context;

		private ICompetitorRepository _competitorRepository;
		private IPlayerRepository _playerRepository;
		private IRoundRepository _roundRepository;
		private ITournamentRepository _tournamentRepository;
		private IVenueRepository _venueRepository;

		public ICompetitorRepository CompetitorRepository => GetCompetitorRepository();
		public IPlayerRepository PlayerRepository => GetPlayerRepository();
		public IRoundRepository RoundRepository => GetRoundRepository();
		public ITournamentRepository TournamentRepository => GetTournamentRepository();
		public IVenueRepository VenueRepository => GetVenueRepository();

		public UnitOfWork(TournamentManagementDbContext context)
		{
			_context = context;
		}

		private ICompetitorRepository GetCompetitorRepository()
		{
			if (_competitorRepository == null)
			{
				_competitorRepository = new CompetitorRepository(_context);
			}
			return _competitorRepository;
		}

		private IPlayerRepository GetPlayerRepository()
		{
			if (_playerRepository == null)
			{
				_playerRepository = new PlayerRepository(_context);
			}
			return _playerRepository;
		}

		private IRoundRepository GetRoundRepository()
		{
			if (_roundRepository == null)
			{
				_roundRepository = new RoundRepository(_context);
			}
			return _roundRepository;
		}

		private ITournamentRepository GetTournamentRepository()
		{
			if (_tournamentRepository == null)
			{
				_tournamentRepository = new TournamentRepository(_context);
			}
			return _tournamentRepository;
		}

		private IVenueRepository GetVenueRepository()
		{
			if (_venueRepository == null)
			{
				_venueRepository = new VenueRepository(_context);
			}
			return _venueRepository;
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}
	}
}
