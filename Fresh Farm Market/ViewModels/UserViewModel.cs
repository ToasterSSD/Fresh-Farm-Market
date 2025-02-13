using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

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

		public static UserViewModel FromUser(User user, IDataProtector protector)
		{
			string creditCardNo = string.Empty;
			if (!string.IsNullOrEmpty(user?.CreditCardNo))
			{
				try
				{
					creditCardNo = protector.Unprotect(user.CreditCardNo);
				}
				catch (CryptographicException)
				{
					// Handle the case where the unprotect operation fails
					creditCardNo = "Invalid data";
				}
			}

			return new UserViewModel
			{
				FullName = user?.FullName ?? string.Empty,
				CreditCardNo = creditCardNo,
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
