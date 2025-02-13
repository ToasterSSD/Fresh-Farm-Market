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
				FullName = user.FullName,
				CreditCardNo = EncryptionHelper.Decrypt(user.CreditCardNo),
				Gender = user.Gender,
				MobileNo = user.MobileNo,
				DeliveryAddress = user.DeliveryAddress,
				AboutMe = user.AboutMe,
				Photo = user.Photo,
				Email = user.Email // Set Email property
			};
		}
	}
}
