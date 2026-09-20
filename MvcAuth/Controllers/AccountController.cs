using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuth.Data.Repositories;
using MvcAuth.Models;
using MvcAuth.Services;
using MvcAuth.Services.Results;
using MvcAuth.ViewModels;
using System.Security.Claims;

namespace MvcAuth.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;

		private readonly IAuthService _authService;


		public AccountController(
			IUserService userService,
			IAuthService authService)
		{
			_userService = userService;
			_authService = authService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			RegisterResult result;

			try
			{
				result = await _authService.RegisterAsync(model);
			}
			catch
			{
				ModelState.AddModelError(
					string.Empty,
					"Unable to create the account. Please try again.");

				return View(model);
			}

			if (!result.Success)
			{
				if (result.ErrorCode == "DuplicateAccount")
				{
					ModelState.AddModelError(
						nameof(model.Account),
						"Account already exists.");
				}
				else if (result.ErrorCode == "DuplicateEmail")
				{
					ModelState.AddModelError(
						nameof(model.Email),
						"Email already exists.");
				}

				return View(model);
			}

			return RedirectToAction("Login");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(
			LoginViewModel model,
			string? returnUrl = null)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _authService.ValidateUserAsync(model.Account, model.Password);

			if (user == null)
			{
				ModelState.AddModelError(
					string.Empty,
					"Invalid account or password.");

				return View(model);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Oid.ToString()),
				new Claim(ClaimTypes.Name, user.Account),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var claimsIdentity = new ClaimsIdentity(
				claims,
				CookieAuthenticationDefaults.AuthenticationScheme);

			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			var authProperties = new AuthenticationProperties
			{
				IsPersistent = model.RememberMe
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				claimsPrincipal,
				authProperties);

			if (!string.IsNullOrEmpty(returnUrl) &&
				Url.IsLocalUrl(returnUrl))
			{
				return LocalRedirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Login", "Account");
		}

		[Authorize]
		[HttpGet]
		public IActionResult Profile()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult Admin()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Users()
		{
			var model = await _userService.GetAllAsync();

			return View(model);
		}
	}
}