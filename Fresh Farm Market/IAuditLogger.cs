using System.Threading.Tasks;

public interface IAuditLogger
{
	Task LogAsync(string userId, string activity);
}
