using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class RegisterModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly AuthDbContext _dbContext;
		private readonly PasswordHelper _passwordHelper;

		public RegisterModel(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			AuthDbContext dbContext,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_dbContext = dbContext;
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
			[MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
			[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
			public string Password { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[Compare("Password")]
			public string ConfirmPassword { get; set; }

			[Required]
			public string FullName { get; set; }

			[Required]
			public string CreditCardNo { get; set; }

			[Required]
			public string Gender { get; set; }

			[Required]
			public string MobileNo { get; set; }

			[Required]
			public string DeliveryAddress { get; set; }

			public string AboutMe { get; set; }

			public IFormFile Photo { get; set; }
		}

		public async Task<JsonResult> OnGetCheckEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return new JsonResult(user == null);
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				if (ModelState.IsValid)
				{
					// Check if email already exists
					var existingUser = await _userManager.FindByEmailAsync(Input.Email);
					if (existingUser != null)
					{
						ModelState.AddModelError("Input.Email", "This email is already registered.");
						return Page();
					}

					// Create new user
					var user = new User
					{
						UserName = Input.Email,
						Email = Input.Email,
						FullName = Input.FullName,
						CreditCardNo = EncryptionHelper.Encrypt(Input.CreditCardNo),
						Gender = Input.Gender,
						MobileNo = Input.MobileNo,
						DeliveryAddress = Input.DeliveryAddress,
						AboutMe = Input.AboutMe,
						EmailConfirmed = true 
					};

					
					if (Input.Photo != null && Input.Photo.Length > 0)
					{
						var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
						Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist

						var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.Photo.FileName;
						var filePath = Path.Combine(uploadsFolder, uniqueFileName);

						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await Input.Photo.CopyToAsync(fileStream);
						}

						user.Photo = $"/uploads/{uniqueFileName}";
					}

					// Hash the password with pepper
					var hashedPassword = _passwordHelper.HashPassword(Input.Password);

					// Create the user with hashed password
					var result = await _userManager.CreateAsync(user, hashedPassword);

					if (result.Succeeded)
					{
						// Sign in the user immediately after registration
						await _signInManager.SignInAsync(user, isPersistent: false);

						// Redirect to home page
						return RedirectToPage("/Index");
					}

					// If we got this far, something failed, redisplay form with errors
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			catch (Exception ex)
			{
				
				ModelState.AddModelError(string.Empty, "An error occurred during registration.");
			}

			
			return Page();
		}
	}
}
