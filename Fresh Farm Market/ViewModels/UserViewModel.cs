using Fresh_Farm_Market.Model;

namespace Fresh_Farm_Market.ViewModels
{
	public class UserViewModel
	{
		public string FullName { get; set; }
		public string CreditCardNo { get; set; }
		public string Gender { get; set; }
		public string MobileNo { get; set; }
		public string DeliveryAddress { get; set; }
		public string AboutMe { get; set; }
		public string Photo { get; set; }
		public string Email { get; set; } // Add Email property

		public static UserViewModel FromUser(User user)
		{
			return new UserViewModel
			{
				FullName = user?.FullName ?? string.Empty,
				CreditCardNo = user?.CreditCardNo != null ? EncryptionHelper.Decrypt(user.CreditCardNo) : string.Empty,
				Gender = user?.Gender ?? string.Empty,
				MobileNo = user?.MobileNo ?? string.Empty,
				DeliveryAddress = user?.DeliveryAddress ?? string.Empty,
				AboutMe = user?.AboutMe ?? string.Empty,
				Photo = user?.Photo ?? string.Empty,
				Email = user?.Email ?? string.Empty // Set Email property
			};
		}
	}
}

