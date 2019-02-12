using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Tajes.Resources.Database
{
	public class SqliteDbContext: DbContext
	{
		public DbSet<UniOviUser> UniOviUsers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			string DbLocation = "Data/";
			options.UseSqlite($"Data Source={DbLocation}Database.sqlite");
		}
	}
}
