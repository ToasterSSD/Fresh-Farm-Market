using Fresh_Farm_Market.Model;
using System;
using System.Threading.Tasks;

public class AuditLogger : IAuditLogger
{
	private readonly AuthDbContext _dbContext;

	public AuditLogger(AuthDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task LogAsync(string userId, string activity)
	{
		var userActivity = new UserActivity
		{
			UserId = userId,
			Activity = activity,
			Timestamp = DateTime.UtcNow
		};

		_dbContext.UserActivities.Add(userActivity);
		await _dbContext.SaveChangesAsync();
	}
}
