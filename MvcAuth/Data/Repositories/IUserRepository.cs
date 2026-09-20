using MvcAuth.Models;

namespace MvcAuth.Data.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetByAccountAsync(string account);

		Task<User?> GetByEmailAsync(string email);

		Task<IEnumerable<User>> GetAllAsync();

		Task CreateAsync(User user);
	}
}