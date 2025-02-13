using System.Security.Cryptography;
using System.Text;

namespace Fresh_Farm_Market
{
	public static class EncryptionHelper
	{
		private static readonly string key = "your-encryption-key"; // Use a secure key

		public static string Encrypt(string plainText)
		{
			using (var aes = Aes.Create())
			{
				var keyBytes = Encoding.UTF8.GetBytes(key);
				Array.Resize(ref keyBytes, aes.Key.Length);
				aes.Key = keyBytes;

				var iv = aes.IV;
				using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
				using (var ms = new MemoryStream())
				{
					ms.Write(iv, 0, iv.Length);
					using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					using (var sw = new StreamWriter(cs))
					{
						sw.Write(plainText);
					}
					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public static string Decrypt(string cipherText)
		{
			var fullCipher = Convert.FromBase64String(cipherText);
			using (var aes = Aes.Create())
			{
				var iv = new byte[aes.IV.Length];
				Array.Copy(fullCipher, iv, iv.Length);
				var keyBytes = Encoding.UTF8.GetBytes(key);
				Array.Resize(ref keyBytes, aes.Key.Length);
				aes.Key = keyBytes;

				using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
				using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
				using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
				using (var sr = new StreamReader(cs))
				{
					return sr.ReadToEnd();
				}
			}
		}
	}

}
