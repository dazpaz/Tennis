using DomainDesign.Common;

namespace Players.Domain.PlayerAggregate;

public sealed class PlayerId : EntityId<PlayerId>
{
	public PlayerId() : base() { }
	public PlayerId(Guid id) : base(id) { }
}
