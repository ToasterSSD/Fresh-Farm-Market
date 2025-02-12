// Fresh_Farm_Market/Model/UserActivity.cs
using Fresh_Farm_Market.Model;

public class UserActivity
{
	public int? Id { get; set; }
	public string? UserId { get; set; }
	public string? Activity { get; set; }
	public DateTime? Timestamp { get; set; }
	public User? User { get; set; }
}
