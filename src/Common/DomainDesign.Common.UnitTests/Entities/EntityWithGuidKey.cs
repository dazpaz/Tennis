using DomainDesign.Common;
using System;

namespace DomainDesign.Kernel.UnitTests.Entities
{
	public class EntityWithGuidKey : Entity<Guid>
	{
		public string Name { get; set; }

		public EntityWithGuidKey(Guid id) : base(id)
		{
		}

		public EntityWithGuidKey() : base()
		{
		}
	}
}
