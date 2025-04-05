using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Rent_A_Car.Models;
using Rent_A_Car.ViewsUserOperations;

namespace Rent_A_Car.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.roleManager = roleManager;
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginView model)
		{
			if (ModelState.IsValid)
			{
				var existingUser = await userManager.FindByEmailAsync(model.Email);

				if (existingUser.Password == model.Password)
				{
					await signInManager.SignInAsync(existingUser, isPersistent: model.RememberMe);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Email or password is incorrect.");
					return View(model);
				}
			}
			return View(model);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterView model)
		{
			if (ModelState.IsValid)
			{
				var existingUser = await userManager.FindByEmailAsync(model.Email);

				if (existingUser == null)
				{
					User user = new User
					{
						UserName = model.UserName,
						FirstName = model.FirstName,
						LastName = model.LastName,
						EGN = model.EGN,
						PhoneNumber = model.PhoneNumber,
						Email = model.Email,
						Password = model.Password
					};

					var adminRole = "Admin";
					roleManager.CreateAsync(new IdentityRole(adminRole)).Wait();

					var result = await userManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
					{
						if (user.Email == "admin@gmail.com")
						{
							await userManager.AddToRoleAsync(user, adminRole);
							return RedirectToAction("Login", "Account");
						}
						else
						{
							return RedirectToAction("Login", "Account");
						}
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
						return View(model);
					}
				}
				else if (existingUser.EGN == model.EGN || existingUser.UserName == model.UserName || existingUser.Email == model.Email)
				{
					ModelState.AddModelError(string.Empty, "This user already exists.");
				}
			}
			return View(model);
		}
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public IActionResult VerifyEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> VerifyEmail(VerifyEmailView model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);

				if (user == null)
				{
					ModelState.AddModelError("", "Email is incorrect!");
					return View(model);
				}
				else
				{
					return RedirectToAction("ChangePassword", "Account", new { email = user.Email });
				}
			}
			return View(model);
		}
		public IActionResult ChangePassword(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("VerifyEmail", "Account");
			}
			return View(new ChangePasswordView { Email = email });
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordView model)
		{
			if (ModelState.IsValid)
			{
				var userToEdit = await userManager.FindByEmailAsync(model.Email);

				if (userToEdit != null)
				{
					userToEdit.Password = model.NewPassword;
					var result = await userManager.RemovePasswordAsync(userToEdit);
					if (result.Succeeded)
					{
						result = await userManager.AddPasswordAsync(userToEdit, model.NewPassword);
						return RedirectToAction("Login", "Account");
					}
					else
					{

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}

						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("", "Email not found!");
					return View(model);
				}
			}
			else
			{
				ModelState.AddModelError("", "Something went wrong. try again.");
				return View(model);
			}
		}
	}
}
