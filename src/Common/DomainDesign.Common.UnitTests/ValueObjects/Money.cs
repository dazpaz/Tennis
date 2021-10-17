namespace DomainDesign.Common.UnitTests.ValueObjects
{
	class Money : ValueObject<Money>
	{
		public int Amount { get; private set; }
		public string Currency { get; private set; }

		public Money(int amount, string currency)
		{
			Amount = amount;
			Currency = currency;
		}

		protected override bool ValueObjectEquals(Money other)
		{
			return Amount == other.Amount && Currency == other.Currency;
		}

		protected override int GetValueObjectHashCode()
		{
			return ToString().GetHashCode();
		}

		public override string ToString()
		{
			return $"{Amount} {Currency}";
		}
	}
}
