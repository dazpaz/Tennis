using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;

#nullable disable

namespace Players.Application.Commands
{
	public class UpdateDoublesRankingCommandHandler : ICommandHandler<UpdateDoublesRankingCommand>
	{
		private readonly IUnitOfWork _uow;

		public UpdateDoublesRankingCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(UpdateDoublesRankingCommand command)
		{
			try
			{
				var player = _uow.PlayerRepository.GetById(command.PlayerId);
				if (player == null) return Result.Failure<Guid>("Player does not exist");

				player.UpdateDoublesRank(command.Ranking, command.RankingPoints, command.Date);

				_uow.SaveChanges();

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure<Guid>(ex.Message);
			}
		}
	}
}
