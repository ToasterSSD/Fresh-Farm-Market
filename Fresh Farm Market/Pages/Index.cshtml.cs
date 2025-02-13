using Fresh_Farm_Market.Model;
using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
	private readonly UserManager<User> _userManager;

	public IndexModel(UserManager<User> userManager)
	{
		_userManager = userManager;
	}

	public UserViewModel UserViewModel { get; set; }

	public async Task OnGetAsync()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var user = await _userManager.FindByIdAsync(userId);
		UserViewModel = UserViewModel.FromUser(user);
	}
}
