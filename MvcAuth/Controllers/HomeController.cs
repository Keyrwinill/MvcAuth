using Microsoft.AspNetCore.Mvc;
using MvcAuth.Data.Repositories;
using MvcAuth.Models;
using System.Diagnostics;

namespace MvcAuth.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUserRepository _userRepository;

		public HomeController(
			ILogger<HomeController> logger,
			IUserRepository userRepository)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> TestUser(string account)
		{
			var user = await _userRepository.GetByAccountAsync(account);

			if (user == null)
			{
				return Content("User not found.");
			}

			return Content(
				$"Account: {user.Account}, Email: {user.Email}, Role: {user.Role}");
		}

		[ResponseCache(
			Duration = 0,
			Location = ResponseCacheLocation.None,
			NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			});
		}
	}
}