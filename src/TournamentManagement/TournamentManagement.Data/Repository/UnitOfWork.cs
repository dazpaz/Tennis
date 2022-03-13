using TournamentManagement.Application.Repository;
using TournamentManagement.Domain.CompetitorAggregate;
using TournamentManagement.Domain.PlayerAggregate;
using TournamentManagement.Domain.RoundAggregate;
using TournamentManagement.Domain.TournamentAggregate;
using TournamentManagement.Domain.VenueAggregate;

namespace TournamentManagement.Data.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TournamentManagementDbContext _context;

		IRepository<Competitor, CompetitorId> _competitorRepository;
		IRepository<Player, PlayerId> _playerRepository;
		IRepository<Round, RoundId> _roundRepository;
		IRepository<Tournament, TournamentId> _tournamentRepository;
		IRepository<Venue, VenueId> _venueRepository;

		public IRepository<Competitor, CompetitorId> CompetitorRepository => GetCompetitorRepository();
		public IRepository<Player, PlayerId> PlayerRepository => GetPlayerRepository();
		public IRepository<Round, RoundId> RoundRepository => GetRoundRepository();
		public IRepository<Tournament, TournamentId> TournamentRepository => GetTournamentRepository();
		public IRepository<Venue, VenueId> VenueRepository => GetVenueRepository();

		public UnitOfWork(TournamentManagementDbContext context)
		{
			_context = context;
		}

		private IRepository<Competitor, CompetitorId> GetCompetitorRepository()
		{
			if (_competitorRepository == null)
			{
				_competitorRepository = new Repository<Competitor, CompetitorId >(_context);
			}
			return _competitorRepository;
		}

		private IRepository<Player, PlayerId> GetPlayerRepository()
		{
			if (_playerRepository == null)
			{
				_playerRepository = new Repository<Player, PlayerId>(_context);
			}
			return _playerRepository;
		}

		private IRepository<Round, RoundId> GetRoundRepository()
		{
			if (_roundRepository == null)
			{
				_roundRepository = new Repository<Round, RoundId>(_context);
			}
			return _roundRepository;
		}

		private IRepository<Tournament, TournamentId> GetTournamentRepository()
		{
			if (_tournamentRepository == null)
			{
				_tournamentRepository = new TournamentRepository(_context);
			}
			return _tournamentRepository;
		}

		private IRepository<Venue, VenueId> GetVenueRepository()
		{
			if (_venueRepository == null)
			{
				_venueRepository = new Repository<Venue, VenueId>(_context);
			}
			return _venueRepository;
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}
	}
}
