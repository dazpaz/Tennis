using Cqrs.Common.Data;
using Dapper;
using DomainDesign.Common;
using Microsoft.Data.SqlClient;
using Players.Contract;

#nullable disable

namespace Players.Query
{
	public sealed class GetCountryDetailsList : IQuery<IList<CountryDetailsDto>>
	{
	}

	public sealed class GetCountryDetailsListHandler
		: IQueryHandler<GetCountryDetailsList, IList<CountryDetailsDto>>
	{
		private readonly ConnectionString _connectionString;

		public GetCountryDetailsListHandler(ConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public IList<CountryDetailsDto> Handle(GetCountryDetailsList query)
		{
			var sql = GetSqlQuery();

			using SqlConnection connection = new(_connectionString.Value);

			return connection.Query<CountryDetailsDto>(sql)
				.ToList();
		}

		private static string GetSqlQuery()
		{
			return @"SELECT c.Id, c.ShortName, c.FullName
			FROM dbo.Country c";
		}
	}
}
