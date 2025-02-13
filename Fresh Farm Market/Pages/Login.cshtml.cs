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
					var hashedPassword = _passwordHelper.HashPassword(Input.Password);
					var result = await _signInManager.PasswordSignInAsync(user, hashedPassword, false, lockoutOnFailure: true);

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
						ModelState.AddModelError(string.Empty, "Account locked out due to multiple failed login attempts.");
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
