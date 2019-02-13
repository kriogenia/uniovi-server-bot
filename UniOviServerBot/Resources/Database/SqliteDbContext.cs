using Microsoft.EntityFrameworkCore;
using System;

namespace Tajes.Resources.Database
{
	public class SqliteDbContext: DbContext
	{
		public DbSet<UniOviUser> UniOviUsers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			string DbLocation = Environment.CurrentDirectory + "/../../../Data/";
			options.UseSqlite($"Data Source={DbLocation}Database.sqlite");
		}
	}
}
