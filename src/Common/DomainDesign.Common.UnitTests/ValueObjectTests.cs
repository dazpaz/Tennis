using DomainDesign.Common.UnitTests.ValueObjects;
using FluentAssertions;
using System;
using Xunit;

namespace DomainDesign.Common.UnitTests
{
	public class ValueObjectTests
	{
		[Fact]
		public void AddressEqualsWorksWithIdenticalAddresses()
		{
			Address address = new("Address1", "Austin", "TX");
			Address address2 = new("Address1", "Austin", "TX");

			address.Equals(address2).Should().BeTrue();
		}

		[Fact]
		public void AddressEqualsWorksWithNonIdenticalAddresses()
		{
			Address address = new("Address1", "Austin", "TX");
			Address address2 = new("Address2", "Austin", "TX");

			address.Equals(address2).Should().BeFalse();
		}

		[Fact]
		public void AddressEqualsWorksWithNulls()
		{
			Address address = new(null, "Austin", "TX");
			Address address2 = new("Address2", "Austin", "TX");

			address.Equals(address2).Should().BeFalse();
		}

		[Fact]
		public void AddressEqualsWorksWithPairOfNulls()
		{
			Address address = new("Address1", "Austin", null);
			Address address2 = new("Address1", "Austin", null);

			address.Equals(address2).Should().BeTrue();
		}

		[Fact]
		public void AddressEqualsWorksWithNullsOnOtherObject()
		{
			Address address = new("Address2", "Austin", "TX");
			Address address2 = new("Address2", null, "TX");

			address.Equals(address2).Should().BeFalse();
		}

		[Fact]
		public void AddressEqualsIsReflexive()
		{
			Address address = new("Address1", "Austin", "TX");

			address.Equals(address).Should().BeTrue();
		}

		[Fact]
		public void AddressEqualsIsSymmetric()
		{
			Address address = new("Address1", "Austin", "TX");
			Address address2 = new("Address2", "Austin", "TX");

			address.Equals(address2).Should().BeFalse();
			address2.Equals(address).Should().BeFalse();
		}

		[Fact]
		public void AddressEqualsIsTransitive()
		{
			Address address = new("Address1", "Austin", "TX");
			Address address2 = new("Address1", "Austin", "TX");
			Address address3 = new("Address1", "Austin", "TX");

			address.Equals(address2).Should().BeTrue();
			address2.Equals(address3).Should().BeTrue();
			address.Equals(address3).Should().BeTrue();
		}

		[Fact]
		public void AddressOperatorsWork()
		{
			Address address = new("Address1", "Austin", "TX");
			Address address2 = new("Address1", "Austin", "TX");
			Address address3 = new("Address2", "Austin", "TX");

			(address == address2).Should().BeTrue();
			(address2 != address3).Should().BeTrue();
		}

		[Fact]
		public void DerivedTypesBehaveCorrectly()
		{
			Address address = new("Address1", "Austin", "TX");
			ExpandedAddress address2 = new("Address1", "Apt 123", "Austin", "TX");

			address.Equals(address2).Should().BeFalse();
			(address == address2).Should().BeFalse();
		}

		[Fact]
		public void EqualValueObjectsHaveSameHashCode()
		{
			var hashCode1 = new Address("Address1", "Austin", "TX").GetHashCode();
			var hashCode2 = new Address("Address1", "Austin", "TX").GetHashCode();

			hashCode1.Should().Be(hashCode2);
		}

		[Fact]
		public void TransposedValuesGiveDifferentHashCodes()
		{
			var hashCode1 = new Address(null, "Austin", "TX").GetHashCode();
			var hashCode2 = new Address("TX", "Austin", null).GetHashCode();

			hashCode1.Should().NotBe(hashCode2);
		}

		[Fact]
		public void UnequalValueObjectsHaveDifferentHashCodes()
		{
			var hashCode1 = new Address("Address1", "Austin", "TX").GetHashCode();
			var hashCode2 = new Address("Address2", "Austin", "TX").GetHashCode();

			hashCode1.Should().NotBe(hashCode2);
		}

		[Fact]
		public void TransposedValuesOfFieldNamesGivesDifferentHashCodes()
		{
			var hashCode1 = new Address("_city", null, null).GetHashCode();
			var hashCode2 = new Address(null, "_address1", null).GetHashCode();

			hashCode1.Should().NotBe(hashCode2);
		}

		[Fact]
		public void DerivedTypesHashCodesBehaveCorrectly()
		{
			var hashCode1 = new ExpandedAddress("Address99999", "Apt 123", "New Orleans", "LA")
				.GetHashCode();
			var hashCode2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX")
				.GetHashCode();

			hashCode1.Should().NotBe(hashCode2);
		}

		[Fact]
		public void AddressOperatorsWorkWithNullValues()
		{
			Address address1 = null;
			Address address2 = null;
			Address address3 = new("Address2", "Austin", "TX");
			Address address4 = new("Address2", "Austin", "TX");

			(address1 == address2).Should().BeTrue();
			(address1 == address3).Should().BeFalse();
			(address3 == address1).Should().BeFalse();
			(address3 == address4).Should().BeTrue();
		}
	}
}
