namespace DomainDesign.Common.UnitTests.ValueObjects
{
	public class Address : ValueObject<Address>
	{
		public string AddressLine1 { get; }
		public string City { get; }
		public string State { get; }

		public Address(string address1, string city, string state)
		{
			AddressLine1 = address1;
			City = city;
			State = state;
		}
	}

	public class ExpandedAddress : Address
	{
		public string AddressLine2 { get; }

		public ExpandedAddress(string address1, string address2, string city, string state)
			: base(address1, city, state)
		{
			AddressLine2 = address2;
		}
	}
}
