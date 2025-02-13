using System;

namespace Fresh_Farm_Market.Model
{
	public class PasswordHistory
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string HashedPassword { get; set; }
		public DateTime DateSet { get; set; }

		public User User { get; set; }
	}
}
