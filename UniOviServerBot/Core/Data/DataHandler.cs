using Discord;
using System.Collections;
using System.Linq;
using Tajes.Resources.Database;

namespace Tajes.Core.Data
{
	/// <summary>
	/// Gestiona las llamadas a la base de datos
	/// </summary>
	public static class DataHandler
	{
		/// <summary>
		/// Comprueba si el usuario introducido existe ya en la base de datos
		/// </summary>
		/// <returns></returns>
		public static bool CheckUser(ulong userId) {
			using (var db = new SqliteDbContext()) {
				// Si hay menos de una entrada con esa id, el usuario no está aún en la base de datos
				return db.UniOviUsers.Where(x => x.UserID == userId).Count() < 1 ? false : true;
			}
		}

		/// <summary>
		/// Busca los usuarios de la base de datos que juegan al LoL y devuelve una array con sus ID
		/// </summary>
		/// <returns></returns>
		internal static IList GetLolPlayers(IUser user) {
			using (var db = new SqliteDbContext()) {
				// Si no se ha introducido un usuario parámetro, se listarán todos los usuarios
				if (user == null)
					return db.UniOviUsers.Where(x => x.LoLUsername != "").ToList();
				else
					return db.UniOviUsers.Where(x => x.UserID == user.Id).ToList();
			}
		}

		/// <summary>
		/// Crea una entrada en la base de datos
		/// </summary>
		/// <param name="userId">ID del usuario, único para cada uno, clave de la tabla</param>
		/// <param name="username">Nombre de usuario en Discord</param>
		/// <param name="lolUsername">Nombre de invocador en League of Legends</param>
		public static async void Add(ulong userId, string username, string lolUsername = "") {
			using (var db = new SqliteDbContext()) {
				db.UniOviUsers.Add(new UniOviUser {
					UserID = userId,
					Username = username,
					LoLUsername = lolUsername
				});
				await db.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Devuelve el nombre de invocador vinculado al usuario seleccionado
		/// </summary>
		/// <param name="userId">Id del usuario del que buscar el nombre de invocador</param>
		/// <returns></returns>
		public static string GetLolUsername(ulong userId) {
			using (var db = new SqliteDbContext()) {
				return db.UniOviUsers.Where(x => x.UserID == userId).FirstOrDefault().LoLUsername;
			}
		}

		/// <summary>
		/// Modifica el nombre de invocador vinculado al usuario seleccionado
		/// </summary>
		/// <param name="username">Nuevo nombre de invocador a vincular</param>
		public static async void SetLolUsername(ulong userId, string username) {
			using (var db = new SqliteDbContext()) {
				UniOviUser user = db.UniOviUsers.Where(x => x.UserID == userId).FirstOrDefault();
				user.LoLUsername = username;
				db.UniOviUsers.Update(user);
				await db.SaveChangesAsync();
			}
		}
	}
}
