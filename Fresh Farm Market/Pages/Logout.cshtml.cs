using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

public class LogoutModel : PageModel
{
	private readonly SignInManager<User> _signInManager;
	private readonly IAuditLogger _auditLogger;

	public LogoutModel(SignInManager<User> signInManager, IAuditLogger auditLogger)
	{
		_signInManager = signInManager;
		_auditLogger = auditLogger;
	}

	public async Task<IActionResult> OnPostAsync()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		await _signInManager.SignOutAsync();
		await _auditLogger.LogAsync(userId, "User logged out");

		// Clear session
		HttpContext.Session.Clear();

		return RedirectToPage("/Login");
	}
}
