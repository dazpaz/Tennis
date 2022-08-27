using DomainDesign.Common;

namespace Players.Domain.CountryAggregate
{
	public sealed class CountryId : EntityId<CountryId>
	{
		public CountryId() : base() { }
		public CountryId(Guid id) : base(id) { }
	}
}
