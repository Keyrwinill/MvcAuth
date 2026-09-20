using MvcAuth.Models;
using MvcAuth.Services.Results;
using MvcAuth.ViewModels;

namespace MvcAuth.Services
{
	public interface IAuthService
	{
		Task<User?> ValidateUserAsync(string account, string password);

		Task<RegisterResult> RegisterAsync(RegisterViewModel model);
	}
}