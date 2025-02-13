using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AuthDbContext : IdentityDbContext<User>
{
	private readonly IConfiguration _configuration;

	public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration)
		: base(options)
	{
		_configuration = configuration;
	}

	public DbSet<User> Users { get; set; }
	public DbSet<UserActivity> UserActivities { get; set; }
	public DbSet<PasswordHistory> PasswordHistories { get; set; }


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			string connectionString = _configuration.GetConnectionString("AuthConnectionString");
			optionsBuilder.UseSqlServer(connectionString);
		}
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<UserActivity>()
			.HasOne(ua => ua.User)
			.WithMany(u => u.UserActivities)
			.HasForeignKey(ua => ua.UserId);
	}
}
