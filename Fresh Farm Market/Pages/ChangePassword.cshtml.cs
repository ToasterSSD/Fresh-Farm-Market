using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class ChangePasswordModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly AuthDbContext _dbContext;
		private readonly PasswordHelper _passwordHelper;

		public ChangePasswordModel(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			AuthDbContext dbContext,
			PasswordHelper passwordHelper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_dbContext = dbContext;
			_passwordHelper = passwordHelper;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			[DataType(DataType.Password)]
			public string CurrentPassword { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
			[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
			public string NewPassword { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[Compare("NewPassword", ErrorMessage = "Passwords do not match")]
			public string ConfirmPassword { get; set; }
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			// Check password history
			var hashedNewPassword = _passwordHelper.HashPassword(Input.NewPassword);
			var passwordHistory = _dbContext.PasswordHistories
				.Where(ph => ph.UserId == user.Id)
				.OrderByDescending(ph => ph.DateSet)
				.Take(2)
				.ToList();

			if (passwordHistory.Any(ph => ph.HashedPassword == hashedNewPassword))
			{
				ModelState.AddModelError(string.Empty, "You cannot reuse your last 2 passwords.");
				return Page();
			}

			var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				foreach (var error in changePasswordResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return Page();
			}

			// Add new password to password history
			var newPasswordHistory = new PasswordHistory
			{
				UserId = user.Id,
				HashedPassword = hashedNewPassword,
				DateSet = DateTime.UtcNow
			};
			_dbContext.PasswordHistories.Add(newPasswordHistory);
			await _dbContext.SaveChangesAsync();

			await _signInManager.RefreshSignInAsync(user);
			return RedirectToPage("/Index");
		}
	}
}
