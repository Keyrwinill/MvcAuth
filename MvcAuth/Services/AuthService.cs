using MvcAuth.Data.Repositories;
using MvcAuth.Models;
using MvcAuth.Services.Results;
using MvcAuth.ViewModels;

namespace MvcAuth.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly PasswordService _passwordService;

		public AuthService(
			IUserRepository userRepository,
			PasswordService passwordService)
		{
			_userRepository = userRepository;
			_passwordService = passwordService;
		}

		public async Task<User?> ValidateUserAsync(
			string account,
			string password)
		{
			var user = await _userRepository.GetByAccountAsync(account);

			if (user == null)
			{
				return null;
			}

			bool passwordValid =
				_passwordService.VerifyPassword(
					password,
					user.PasswordHash);

			if (!passwordValid)
			{
				return null;
			}

			return user;
		}

		public async Task<RegisterResult> RegisterAsync(RegisterViewModel model)
		{
			var existingAccount =
				await _userRepository.GetByAccountAsync(model.Account);

			if (existingAccount != null)
			{
				return new RegisterResult
				{
					Success = false,
					ErrorCode = "DuplicateAccount"
				};
			}

			var existingEmail =
				await _userRepository.GetByEmailAsync(model.Email);

			if (existingEmail != null)
			{
				return new RegisterResult
				{
					Success = false,
					ErrorCode = "DuplicateEmail"
				};
			}

			var user = new User
			{
				Oid = Guid.NewGuid(),
				Account = model.Account,
				Email = model.Email,
				PasswordHash = _passwordService.HashPassword(model.Password),
				CreatedDate = DateTime.UtcNow,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Birthday = model.Birthday,
				Role = "User"
			};

			await _userRepository.CreateAsync(user);

			return new RegisterResult
			{
				Success = true,
				User = user
			};
		}
	}
}