using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

		public RegisterModel(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			AuthDbContext dbContext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_dbContext = dbContext;
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

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = new User
				{
					UserName = Input.Email,
					Email = Input.Email,
					FullName = Input.FullName,
					CreditCardNo = Input.CreditCardNo,
					Gender = Input.Gender,
					MobileNo = Input.MobileNo,
					DeliveryAddress = Input.DeliveryAddress,
					AboutMe = Input.AboutMe
					// Remove Password property assignment - let UserManager handle it
				};

				if (Input.Photo != null)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
					if (!Directory.Exists(uploadsFolder))
					{
						Directory.CreateDirectory(uploadsFolder);
					}

					var filePath = Path.Combine(uploadsFolder, Input.Photo.FileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await Input.Photo.CopyToAsync(stream);
					}
					user.Photo = $"/uploads/{Input.Photo.FileName}";
				}

				// UserManager.CreateAsync handles password hashing internally
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					var userActivity = new UserActivity
					{
						UserId = user.Id,
						Activity = "User Registration",
						Timestamp = DateTime.UtcNow
					};
					_dbContext.UserActivities.Add(userActivity);
					await _dbContext.SaveChangesAsync();

					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToPage("Index");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return Page();
		}

	}
}
