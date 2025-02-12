using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Fresh_Farm_Market.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.Text)]
		public string FullName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		public string CreditCardNo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid mobile number.")]
		public string MobileNo { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string DeliveryAddress { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[DataType(DataType.Upload)]
		[RegularExpression(@"^.*\.jpg$", ErrorMessage = "Only .jpg files are allowed.")]
		public string Photo { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string AboutMe { get; set; }
	}
}
