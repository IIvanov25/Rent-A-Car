using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<IActionResult> BrowseUsers()
	{
		var users = _userManager.Users.ToList();
		return View(users);
	}

	public async Task<IActionResult> Edit(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user == null) return NotFound();
		return View(user);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(User model)
	{
		var user = await _userManager.FindByIdAsync(model.Id);
		if (user == null) return NotFound();

		user.UserName = model.UserName;
		user.Email = model.Email;
		user.FirstName = model.FirstName;
		user.LastName = model.LastName;
		user.EGN = model.EGN;
		user.PhoneNumber = model.PhoneNumber;

		var result = await _userManager.UpdateAsync(user);
		if (result.Succeeded)
			return RedirectToAction("BrowseUsers");

		return View(user);
	}
	// GET: User/Delete/{id}
	[HttpGet]
	public async Task<IActionResult> Delete(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user == null) return NotFound(); // If user is not found, return 404
		return View(user); // Render the Delete view
	}

	// POST: User/DeleteConfirmed
	[HttpPost]
	[ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user == null) return NotFound(); // If user not found, return 404

		var result = await _userManager.DeleteAsync(user);
		if (result.Succeeded)
			return RedirectToAction("BrowseUsers"); // Redirect to users list after successful deletion

		return View(user); // Return to the Delete view if deletion failed
	}
}