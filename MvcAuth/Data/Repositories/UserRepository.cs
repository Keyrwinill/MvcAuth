using Dapper;
using MvcAuth.Models;

namespace MvcAuth.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly SqlConnectionFactory _connectionFactory;

		public UserRepository(SqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<User?> GetByAccountAsync(string account)
		{
			const string sql = """
                SELECT
                    Oid,
                    Account,
                    Email,
                    PasswordHash,
                    CreatedDate,
                    FirstName,
                    LastName,
                    Birthday,
                    Role
                FROM Users
                WHERE Account = @Account;
                """;

			using var connection = _connectionFactory.CreateConnection();

			return await connection.QuerySingleOrDefaultAsync<User>(
				sql,
				new { Account = account });
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			const string sql = """
                SELECT
                    Oid,
                    Account,
                    Email,
                    PasswordHash,
                    CreatedDate,
                    FirstName,
                    LastName,
                    Birthday,
                    Role
                FROM Users
                WHERE Email = @Email;
                """;

			using var connection = _connectionFactory.CreateConnection();

			return await connection.QuerySingleOrDefaultAsync<User>(
				sql,
				new { Email = email });
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			const string sql = """
                SELECT
                    Oid,
                    Account,
                    Email,
                    PasswordHash,
                    CreatedDate,
                    FirstName,
                    LastName,
                    Birthday,
                    Role
                FROM Users
                ORDER BY CreatedDate DESC;
                """;

			using var connection = _connectionFactory.CreateConnection();

			return await connection.QueryAsync<User>(sql);
		}

		public async Task CreateAsync(User user)
		{
			const string sql = """
                INSERT INTO Users
                (
                    Oid,
                    Account,
                    Email,
                    PasswordHash,
                    CreatedDate,
                    FirstName,
                    LastName,
                    Birthday,
                    Role
                )
                VALUES
                (
                    @Oid,
                    @Account,
                    @Email,
                    @PasswordHash,
                    @CreatedDate,
                    @FirstName,
                    @LastName,
                    @Birthday,
                    @Role
                );
                """;

			using var connection = _connectionFactory.CreateConnection();

			await connection.ExecuteAsync(sql, user);
		}
	}
}