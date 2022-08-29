using Cqrs.Common.Data;
using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using Players.Contract;

#nullable disable

namespace Players.Query
{
	public sealed class GetCountryDetails : IQuery<CountryDetailsDto>
	{
		public Guid CountryId { get; }

		public GetCountryDetails(Guid countryId)
		{
			CountryId = countryId;
		}
	}

	public sealed class GetCountryDetailsHandler
		: IQueryHandler<GetCountryDetails, CountryDetailsDto>
	{
		private readonly ConnectionString _connectionString;

		public GetCountryDetailsHandler(ConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public CountryDetailsDto Handle(GetCountryDetails query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString.Value);

			return connection.QuerySingle<CountryDetailsDto>(sql, new
			{
				query.CountryId
			});
		}

		private static string GetSqlQuery()
		{
			return @"SELECT c.Id, c.ShortName, c.FullName
			FROM dbo.Country c
			WHERE c.Id = @CountryId";
		}
	}
}
