using Microsoft.Data.SqlClient;
using System.Data;

namespace MvcAuth.Data
{
	public class SqlConnectionFactory
	{
		private readonly string _connectionString;

		public SqlConnectionFactory(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException(
					"Connection string 'DefaultConnection' was not found.");
		}

		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}