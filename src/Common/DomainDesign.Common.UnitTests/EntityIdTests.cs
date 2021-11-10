using FluentAssertions;
using System;
using Xunit;

namespace DomainDesign.Common.UnitTests
{
	public class DomainEntityId : EntityId
	{
		public DomainEntityId() : base() { }
		public DomainEntityId(Guid id) : base(id) { }
	}

	public class EntityIdTests
	{
		[Fact]
		public void CanCreateDomainEntityIdWithNoParameters()
		{
			var entityId = new DomainEntityId();

			entityId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateDomainEntityIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var matchId = new DomainEntityId(id);

			matchId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateDomainEntityIdUsingAnEmptyGuid()
		{
			Action act = () => new DomainEntityId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}

		[Fact]
		public void EqualityWorksForDomainEntityIds()
		{
			var id = Guid.NewGuid();

			var id1 = new DomainEntityId(id);
			var id2 = new DomainEntityId(id);

			(id1 == id2).Should().BeTrue();
			(id1 != id2).Should().BeFalse();
			id1.Equals(id2).Should().BeTrue();
			id1.Equals(null).Should().BeFalse();
		}

		[Fact]
		public void GetHashCodeWorksForDomainEntityIds()
		{
			var id = Guid.NewGuid();

			var id1 = new DomainEntityId(id);
			var id2 = new DomainEntityId(id);

			id1.GetHashCode().Should().Be(id2.GetHashCode());
		}
	}
}
