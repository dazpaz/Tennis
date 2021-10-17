using DomainDesign.Kernel.UnitTests.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace DomainDesign.Common.UnitTests
{
	public class EntityTests
	{
		private const string DefaultKeyExceptionMessage = "The ID cannot be the type's default value. (Parameter 'id')";

		[Fact]
		public void CanCreateAnEntityWithGuidKey()
		{
			var guid = Guid.NewGuid();
			var entity = new EntityWithGuidKey(guid);

			entity.Id.Should().Be(guid);
		}

		[Fact]
		public void CanCreateAnEntityWithIntegerKey()
		{
			var key = 42;
			var entity = new EntityWithIntegerKey(key);

			entity.Id.Should().Be(key);
		}

		[Fact]
		public void CannotCreateWithDefaultValueForGuidKey()
		{
			Action act = () => new EntityWithGuidKey(Guid.Empty);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(DefaultKeyExceptionMessage);
		}

		[Fact]
		public void CannotCreateWithDefaultValueForIntegerKey()
		{
			Action act = () => new EntityWithIntegerKey(0);

			act.Should()
				.Throw<ArgumentException>()
				.WithMessage(DefaultKeyExceptionMessage);
		}

		[Fact]
		public void IdentityEquals_EquatableReturnsTrueWhenGuidKeysAreEqual()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			var entity2 = new EntityWithGuidKey(guid);

			entity1.Equals(entity2).Should().BeTrue();
		}

		[Fact]
		public void IdentityEquals_EquatableReturnsFalseWhenGuidKeysAreDifferent()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			var entity2 = new EntityWithGuidKey(Guid.NewGuid());

			entity1.Equals(entity2).Should().BeFalse();
		}

		[Fact]
		public void IdentityEquals_EquatableReturnsFalseWhenComparedToNull()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			EntityWithGuidKey entity2 = null;

			entity1.Equals(entity2).Should().BeFalse();
		}

		[Fact]
		public void OperatorEquals_ReturnsTrueWhenBothOperandsAreNull()
		{
			EntityWithGuidKey entity1 = null;
			EntityWithGuidKey entity2 = null;

			(entity1 == entity2).Should().BeTrue();
		}

		[Fact]
		public void OperatorEquals_ReturnsFalseWhenOneOperandIsNull()
		{
			EntityWithGuidKey entity1 = new EntityWithGuidKey(Guid.NewGuid());
			EntityWithGuidKey entity2 = null;

			(entity1 == entity2).Should().BeFalse();
			(entity2 == entity1).Should().BeFalse();
		}

		[Fact]
		public void OperatorEquals_ReturnsTrueWhenTheEntitiesHaveIdentityEquality()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			var entity2 = new EntityWithGuidKey(guid);

			(entity1 == entity2).Should().BeTrue();
		}

		[Fact]
		public void OperatorEquals_ReturnsFalseWhenTheEntitiesDontHaveIdentityEquality()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			var entity2 = new EntityWithGuidKey(Guid.NewGuid());

			(entity1 == entity2).Should().BeFalse();
		}

		[Fact]
		public void OperatorNotEquals_ReturnsInverseOfOperatorEquals()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			var entity2 = new EntityWithGuidKey(guid);

			(entity1 == entity2).Should().BeTrue();
			(entity1 != entity2).Should().BeFalse();
		}

		[Fact]
		public void GetHashCode_ReturnsSameHashCodeForEquivalentEntities()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			var entity2 = new EntityWithGuidKey(guid);

			var hashCode1 = entity1.GetHashCode();
			var hashCode2 = entity2.GetHashCode();

			(hashCode1 == hashCode2).Should().BeTrue();
		}

		[Fact]
		public void GetHashCode_ReturnsDifferentHashCodeForUnequivalentEntities()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			var entity2 = new EntityWithGuidKey(Guid.NewGuid());

			var hashCode1 = entity1.GetHashCode();
			var hashCode2 = entity2.GetHashCode();

			(hashCode1 == hashCode2).Should().BeFalse();
		}

		[Fact]
		public void Equals_EntitiesWithDifferentKeyTypeAreNotEqual()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			var entity2 = new EntityWithIntegerKey(42);

			entity1.Equals(entity2).Should().BeFalse();
		}


		[Fact]
		public void Equals_TwoReferencesToTheSameObjectAreEqual()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			object entity2 = entity1;

			entity1.Equals(entity2).Should().BeTrue();
		}

		[Fact]
		public void Equals_EntitiesOfDifferentTypesAreNotEqual()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			object entity2 = new AnotherEntityWithGuidKey(guid);

			entity1.Equals(entity2).Should().BeFalse();
		}

		[Fact]
		public void Equals_EntityWithDefaultKeyIsNotEqual()
		{
			var entity1 = new EntityWithGuidKey(Guid.NewGuid());
			object entity2 = new EntityWithGuidKey();

			entity1.Equals(entity2).Should().BeFalse();
		}

		[Fact]
		public void Equals_EqualEntitiesAreConsideredEqual()
		{
			var guid = Guid.NewGuid();
			var entity1 = new EntityWithGuidKey(guid);
			object entity2 = new EntityWithGuidKey(guid);

			entity1.Equals(entity2).Should().BeTrue();
		}
	}
}
