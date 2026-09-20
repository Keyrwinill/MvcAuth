using MvcAuth.ViewModels;

namespace MvcAuth.Services
{
	public interface IUserService
	{
		Task<IEnumerable<UserListItemViewModel>> GetAllAsync();
	}
}