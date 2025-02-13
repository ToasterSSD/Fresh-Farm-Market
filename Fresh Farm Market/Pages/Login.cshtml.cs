using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class LoginModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IAuditLogger _auditLogger;

		public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IAuditLogger auditLogger)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_auditLogger = auditLogger;
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
					var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, false, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						await _auditLogger.LogAsync(user.Id, "User logged in");
						Log.Information("User {UserId} logged in successfully.", user.Id);

						// Create a secure session
						HttpContext.Session.SetString("UserId", user.Id);
						HttpContext.Session.SetString("SecurityStamp", user.SecurityStamp);

						return RedirectToPage("/Index");
					}
					else if (result.IsLockedOut)
					{
						Log.Warning("User {UserId} account locked out due to multiple failed login attempts.", user.Id);
						ModelState.AddModelError(string.Empty, "Account locked out due to multiple failed login attempts. Please try again later.");
						return Page();
					}
					else
					{
						Log.Warning("Invalid login attempt for user {UserId}.", user.Id);
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					}
				}
				else
				{
					Log.Warning("Invalid login attempt for email {Email}.", Input.Email);
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				}
			}

			return Page();
		}
	}
}
