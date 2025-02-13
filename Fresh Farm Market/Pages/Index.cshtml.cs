using Fresh_Farm_Market.Model;
using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
	private readonly UserManager<User> _userManager;
	private readonly IDataProtector _protector;

	public IndexModel(UserManager<User> userManager, IDataProtectionProvider dataProtectionProvider)
	{
		_userManager = userManager;
		_protector = dataProtectionProvider.CreateProtector("FreshFarmMarket.Protector");
	}

	public UserViewModel? UserViewModel { get; set; }

	public async Task OnGetAsync()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userId == null)
		{
			// Handle the case where userId is null
			return;
		}

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			// Handle the case where user is not found
			return;
		}

		UserViewModel = UserViewModel.FromUser(user, _protector);
	}
}
