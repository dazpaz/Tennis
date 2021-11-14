using FluentAssertions;
using System;
using Xunit;

namespace DomainDesign.Common.UnitTests
{
	public sealed class PersonId : EntityId<PersonId>
	{
		public PersonId() : base() { }
		public PersonId(Guid id) : base(id) { }
	}

	public sealed class PlaceId : EntityId<PersonId>
	{
		public PlaceId() : base() { }
		public PlaceId(Guid id) : base(id) { }
	}

	public class EntityIdTests
	{
		[Fact]
		public void CanCreateEntityIdWithNoParameters()
		{
			var entityId = new PersonId();

			entityId.Id.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void CanCreateEntityIdUsingASpecifiedGuid()
		{
			var id = Guid.NewGuid();
			var matchId = new PersonId(id);

			matchId.Id.Should().Be(id);
		}

		[Fact]
		public void CannotCreateEntityIdUsingAnEmptyGuid()
		{
			Action act = () => new PersonId(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("Guid cannot have empty value (Parameter 'id')");
		}

		[Fact]
		public void EqualityOperatorsWorkForEntityIds()
		{
			var id = Guid.NewGuid();

			PersonId nullPerson = null;
			var person1 = new PersonId(id);
			var person2 = new PersonId(id);
			var person3 = new PersonId();
			var place = new PlaceId(id);

			(person1 == nullPerson).Should().BeFalse();
			(nullPerson == person1).Should().BeFalse();
			(nullPerson == null).Should().BeTrue();
			(null == nullPerson).Should().BeTrue();
			(person1 == person2).Should().BeTrue();
			(person1 != person2).Should().BeFalse();
			(person1 == person3).Should().BeFalse();
			(person1 == place).Should().BeFalse();
		}

		[Fact]
		public void GetHashCodeWorksForDomainEntityIds()
		{
			var id = Guid.NewGuid();

			var person1 = new PersonId(id);
			var person2 = new PersonId(id);
			var person3 = new PersonId();
			var place = new PlaceId(id);

			person1.GetHashCode().Should().Be(person2.GetHashCode());
			person1.GetHashCode().Should().NotBe(person3.GetHashCode());
			person1.GetHashCode().Should().NotBe(place.GetHashCode());
		}
	}
}
