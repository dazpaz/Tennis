using Cqrs.Common.Data;
using FluentAssertions;

namespace Cqrs.Common.UnitTests.Data;

public class ConnectionStringTests
{
	[Fact]
	public void CanCreateAConnectionString()
	{
		var connectionString = new ConnectionString("connection-string");
		connectionString.Value.Should().Be("connection-string");
	}
}
