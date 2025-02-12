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

		public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			[DataType(DataType.Text)]
			public string FullName { get; set; }

			[Required]
			[DataType(DataType.CreditCard)]
			public string CreditCardNo { get; set; }

			[Required]
			[DataType(DataType.Text)]
			public string Gender { get; set; }

			[Required]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid mobile number.")]
			public string MobileNo { get; set; }

			[Required]
			[DataType(DataType.MultilineText)]
			public string DeliveryAddress { get; set; }

			[Required]
			[DataType(DataType.EmailAddress)]
			[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{12,}$", ErrorMessage = "Password must contain at least 12 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
			public string Password { get; set; }

			[Required]
			[DataType(DataType.Password)]
			[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[DataType(DataType.Upload)]
			[RegularExpression(@"^.*\.jpg$", ErrorMessage = "Only .jpg files are allowed.")]
			public IFormFile Photo { get; set; }

			[Required]
			[DataType(DataType.MultilineText)]
			public string AboutMe { get; set; }
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
				};

				if (Input.Photo != null)
				{
					var filePath = Path.Combine("wwwroot/uploads", Input.Photo.FileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await Input.Photo.CopyToAsync(stream);
					}
					user.Photo = $"/uploads/{Input.Photo.FileName}";
				}

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
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
