using DomainDesign.Common;

namespace DomainDesign.Kernel.UnitTests.Entities
{
	public class EntityWithIntegerKey : Entity<int>
	{
		public string Name { get; set; }

		public EntityWithIntegerKey(int id) : base(id)
		{
		}
	}
}
