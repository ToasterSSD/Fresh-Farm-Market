using Fresh_Farm_Market.Model;
using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class UserDetailsModel : PageModel
	{
		private readonly UserManager<User> _userManager;

		public UserDetailsModel(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public UserViewModel UserViewModel { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			UserViewModel = UserViewModel.FromUser(user);
			return Page();
		}
	}
}
