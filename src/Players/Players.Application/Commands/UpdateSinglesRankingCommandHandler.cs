using CSharpFunctionalExtensions;
using DomainDesign.Common;
using Players.Application.Repository;

#nullable disable

namespace Players.Application.Commands
{
	public class UpdateSinglesRankingCommandHandler : ICommandHandler<UpdateSinglesRankingCommand>
	{
		private readonly IUnitOfWork _uow;

		public UpdateSinglesRankingCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public Result Handle(UpdateSinglesRankingCommand command)
		{
			try
			{
				var player = _uow.PlayerRepository.GetById(command.PlayerId);
				if (player == null) return Result.Failure<Guid>("Player does not exist");

				player.UpdateSinglesRank(command.Ranking, command.RankingPoints, command.Date);

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
