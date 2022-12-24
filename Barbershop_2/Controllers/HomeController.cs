using Barbershop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Barbershop.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUserStore<IdentityUser> _userStore;
		private readonly IUserEmailStore<IdentityUser> _emailStore;

		public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IUserStore<IdentityUser> userStore)
		{
			_logger = logger;
			_roleManager = roleManager;
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();

		}

		public async Task<IActionResult> CreateAdmin()
		{
			//bool isAdminExists = false;
			//try
			//{
			//	var a = ;
			//}
			//catch
			//{
			//	isAdminExists = true;
			//}

			if (await _userStore.FindByNameAsync("ADMIN@GMAIL.COM", CancellationToken.None) == null)
			{
				var admin = Activator.CreateInstance<IdentityUser>();

				await _userStore.SetUserNameAsync(admin, "Admin@gmail.com", CancellationToken.None);
				await _emailStore.SetEmailAsync(admin, "Admin@gmail.com", CancellationToken.None);
				await _userManager.CreateAsync(admin, "Admin123!");
				_userManager.Options.SignIn.RequireConfirmedAccount = false;

				var role = Activator.CreateInstance<IdentityRole>();
				role.NormalizedName = "ADMIN";
				role.Name = "admin";
				role.Id = "1";
				role.ConcurrencyStamp = Guid.NewGuid().ToString();
				await _roleManager.CreateAsync(role);

				//var user = await _userManager.FindByNameAsync(userName);
				await _userManager.AddToRoleAsync(admin, "admin");
			}
			return RedirectToAction("Index", "Home");
		}

		private IUserEmailStore<IdentityUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<IdentityUser>)_userStore;
		}

		public async Task<IActionResult> Index()
		{
			await CreateAdmin();
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}