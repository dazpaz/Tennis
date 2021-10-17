using DomainDesign.Common;
using System;

namespace DomainDesign.Kernel.UnitTests.Entities
{
	public class AnotherEntityWithGuidKey : Entity<Guid>
	{
		public string Name { get; set; }

		public AnotherEntityWithGuidKey(Guid id) : base(id)
		{
		}
	}
}
