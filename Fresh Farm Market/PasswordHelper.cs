using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;


namespace Fresh_Farm_Market
{

	public class PasswordHelper
	{
		private readonly string _pepper;

		public PasswordHelper(IConfiguration configuration)
		{
			_pepper = configuration["Pepper"];
		}

		public string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var combinedPassword = password + _pepper;
				byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
				return Convert.ToBase64String(hashBytes);
			}
		}

		public bool VerifyPassword(string hashedPassword, string password)
		{
			var hashedInputPassword = HashPassword(password);
			return hashedPassword == hashedInputPassword;
		}
	}

}
