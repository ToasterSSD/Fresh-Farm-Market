using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class ProfileModel : PageModel
	{
		private readonly UserManager<User> _userManager;

		public ProfileModel(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			public string FullName { get; set; }
			public string Email { get; set; }
			public string MobileNo { get; set; }
			public string DeliveryAddress { get; set; }
			public string AboutMe { get; set; }
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			Input = new InputModel
			{
				FullName = user.FullName,
				Email = user.Email,
				MobileNo = user.MobileNo,
				DeliveryAddress = user.DeliveryAddress,
				AboutMe = user.AboutMe
			};

			return Page();
		}
	}
}

