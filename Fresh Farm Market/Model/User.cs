using Microsoft.AspNetCore.Identity;

namespace Fresh_Farm_Market.Model
{
	public class User : IdentityUser
	{

		public string? FullName { get; set; }

		public string? CreditCardNo { get; set; }

		public string? Gender { get; set; }

		public string? MobileNo { get; set; }

		public string? DeliveryAddress { get; set; }

		public string? Photo { get; set; }

		public string? AboutMe { get; set; }
		public ICollection<UserActivity> UserActivities { get; set; }

	}
}
