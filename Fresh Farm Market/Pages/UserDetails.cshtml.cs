using Fresh_Farm_Market.Model;
using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.Pages
{
	public class UserDetailsModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly IDataProtector _protector;

		public UserDetailsModel(UserManager<User> userManager, IDataProtectionProvider dataProtectionProvider)
		{
			_userManager = userManager;
			_protector = dataProtectionProvider.CreateProtector("FreshFarmMarket.Protector");
		}

		public UserViewModel? UserViewModel { get; set; }

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

			UserViewModel = UserViewModel.FromUser(user, _protector);
			return Page();
		}
	}
}