using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fresh_Farm_Market.Model
{
	public class AuthDbContext : IdentityDbContext<User>
	{
		private readonly IConfiguration _configuration;

		public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration)
			: base(options)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				string connectionString = _configuration.GetConnectionString("AuthConnectionString");
				optionsBuilder.UseSqlServer(connectionString);
			}
		}

		public DbSet<User> Users { get; set; } // DbSet for User
	}
}
