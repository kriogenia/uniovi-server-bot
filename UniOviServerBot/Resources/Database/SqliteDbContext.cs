using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Tajes.Resources.Database
{
	public class SqliteDbContext: DbContext
	{
		public DbSet<UniOviUser> UniOviUsers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			string DbLocation = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Data/";
			options.UseSqlite($"Data Source={DbLocation}Database.sqlite");
		}
	}
}
