using MvcAuth.Data.Repositories;
using MvcAuth.ViewModels;

namespace MvcAuth.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<UserListItemViewModel>> GetAllAsync()
		{
			var users = await _userRepository.GetAllAsync();

			return users.Select(user => new UserListItemViewModel
			{
				Oid = user.Oid,
				Account = user.Account,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Birthday = user.Birthday,
				Role = user.Role,
				CreatedDate = user.CreatedDate
			});
		}
	}
}