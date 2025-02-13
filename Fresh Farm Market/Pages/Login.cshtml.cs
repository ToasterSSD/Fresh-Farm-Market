using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class LoginModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IAuditLogger _auditLogger;
		private readonly PasswordHelper _passwordHelper;

		public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IAuditLogger auditLogger, IConfiguration configuration)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_auditLogger = auditLogger;
			_passwordHelper = new PasswordHelper(configuration);
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(Input.Email);
				if (user != null)
				{
					var result = await _signInManager.PasswordSignInAsync(user, Input.Password, false, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						await _auditLogger.LogAsync(user.Id, "User logged in");

						// Create a secure session
						HttpContext.Session.SetString("UserId", user.Id);
						HttpContext.Session.SetString("SecurityStamp", user.SecurityStamp);

						return RedirectToPage("/Index");
					}
					else if (result.IsLockedOut)
					{
						// Check if the lockout period has expired
						var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
						if (lockoutEnd.HasValue && lockoutEnd.Value.UtcDateTime <= DateTime.UtcNow)
						{
							// Reset lockout count and unlock the user
							await _userManager.SetLockoutEndDateAsync(user, null);
							await _userManager.ResetAccessFailedCountAsync(user);

							// Attempt to sign in again
							result = await _signInManager.PasswordSignInAsync(user, Input.Password, false, lockoutOnFailure: true);
							if (result.Succeeded)
							{
								await _auditLogger.LogAsync(user.Id, "User logged in after lockout");

								// Create a secure session
								HttpContext.Session.SetString("UserId", user.Id);
								HttpContext.Session.SetString("SecurityStamp", user.SecurityStamp);

								return RedirectToPage("/Index");
							}
						}

						ModelState.AddModelError(string.Empty, "Account locked out due to multiple failed login attempts. Please try again later.");
						return Page();
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				}
			}

			return Page();
		}
	}
}
