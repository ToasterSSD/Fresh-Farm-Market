using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages.Account
{
	public class LoginWith2faModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly ILogger<LoginWith2faModel> _logger;

		public LoginWith2faModel(SignInManager<User> signInManager, ILogger<LoginWith2faModel> logger)
		{
			_signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public bool RememberMe { get; set; }

		public class InputModel
		{
			[Required]
			[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Text)]
			[Display(Name = "Authenticator code")]
			public string TwoFactorCode { get; set; }

			[Display(Name = "Remember this machine")]
			public bool RememberMachine { get; set; }
		}

		public async Task<IActionResult> OnGetAsync(bool rememberMe)
		{
			var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new InvalidOperationException($"Unable to load two-factor authentication user.");
			}

			RememberMe = rememberMe;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(bool rememberMe)
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new InvalidOperationException($"Unable to load two-factor authentication user.");
			}

			var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

			var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

			if (result.Succeeded)
			{
				_logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
				return LocalRedirect("~/");
			}
			else if (result.IsLockedOut)
			{
				_logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
				return RedirectToPage("./Lockout");
			}
			else
			{
				_logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
				ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
				return Page();
			}
		}
	}
}
