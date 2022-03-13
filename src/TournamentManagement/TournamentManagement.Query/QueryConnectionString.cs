namespace TournamentManagement.Query
{
	public sealed class QueryConnectionString
	{
		public string Value { get; }

		public QueryConnectionString(string value)
		{
			Value = value;
		}
	}
}
