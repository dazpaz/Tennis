using DomainDesign.Common.UnitTests.ValueObjects;
using FluentAssertions;
using System;
using Xunit;

namespace DomainDesign.Common.UnitTests
{
	public class ValueObjectTests
	{
		[Fact]
		public void ValueEquals_ValueObjectsAreEqualIfAllAttributesMatch()
		{
			var money1 = new Money(100, "GBP");
			var money2 = new Money(100, "GBP");

			money1.Equals(money2).Should().BeTrue();
		}

		[Fact]
		public void ValueEquals_ValueObjectsAreNotEqualIfNotAllAttributesMatch()
		{
			var money1 = new Money(100, "GBP");
			var money2 = new Money(101, "GBP");
			var money3 = new Money(100, "USD");

			money1.Equals(money2).Should().BeFalse();
			money1.Equals(money3).Should().BeFalse();
		}

		[Fact]
		public void ValueEquals_ObjectsAreNotEqualIfTheyAreNotTheCorrectType()
		{
			var money = new Money(100, "GBP");
			var other = new DateTime();

			money.Equals(other).Should().BeFalse();
		}

		[Fact]
		public void GetHashCode_ShouldReturnTheSameHasjCodeValueForValueObjectsWhoAreEqual()
		{
			var hashCode1 = new Money(100, "GBP").GetHashCode();
			var hashCode2 = new Money(100, "GBP").GetHashCode();

			(hashCode1 == hashCode2).Should().BeTrue();
		}

		[Fact]
		public void OperatorEquals_TwoNullValueObjectsShouldBeEqual()
		{
			Money money1 = null;
			Money money2 = null;

			(money1 == money2).Should().BeTrue();
		}

		[Fact]
		public void OperatorEquals_IfOneValueObjectIsNullTheyShouldNotBeEqual()
		{
			Money money1 = new Money(100, "GBP");
			Money money2 = null;

			(money1 == money2).Should().BeFalse();
			(money2 == money1).Should().BeFalse();
		}

		[Fact]
		public void OperatorEquals1()
		{
			var money1 = new Money(100, "GBP");
			var money2 = new Money(100, "GBP");

			(money1 == money2).Should().BeTrue();
		}

		[Fact]
		public void OperatorEquals2()
		{
			var money1 = new Money(100, "GBP");
			var money2 = new Money(101, "GBP");
			var money3 = new Money(100, "USD");

			(money1 == money2).Should().BeFalse();
			(money1 == money3).Should().BeFalse();
		}
	}
}
