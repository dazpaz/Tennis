using System;

namespace DomainDesign.Common
{
	public abstract class EntityId : ValueObject<EntityId>
	{
		public readonly Guid Id;

		public EntityId()
		{
			Id = Guid.NewGuid();
		}

		public EntityId(Guid id)
		{
			Guard.ForGuidIsNotEmpty(id, "id");
			Id = id;
		}
	}
}
