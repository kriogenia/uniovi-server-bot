using Discord;
using Discord.Commands;
using System.Collections;
using System.Threading.Tasks;
using Tajes.Core.Data;
using Tajes.Resources.Database;

namespace Tajes.Core.Commands
{
	/// <summary>
	/// Gestiona los comandos hijos del comando lol
	/// </summary>
	public class LoLCommands : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Grupo de comandos lol
		/// </summary>
		[Group("lol"), Alias("lolete"), Summary("Gestiona los datos de los usuarios jugadores de League of Legends")]
		public class LoLGroup : ModuleBase<SocketCommandContext>
		{
			/// <summary>
			/// Lista los datos de uno o todos los usuarios jugadores de League of Legends
			/// </summary>
			/// <param name="user">_Usuario concreto a buscar si se busca uno</param>
			[Command(""), Summary("Lista los datos de uno o todos los usuarios jugadores de League of Legends")]
			public async Task List(IUser user = null) {
				// Comprueba que el usuario introducido existe
				if (user != null && !DataHandler.CheckUser(user.Id))
					await Context.Channel.SendMessageAsync(":x: El nombre de usuario que has introducido no existe en la lista");
				// Obtención de la lista de usuarios que juegan al LoL
				IList list = DataHandler.GetLolPlayers(user);
				// Inicizalición de los campos
				string usuario = "";
				string invocador = "";
				string opggLinkBase = "http://euw.op.gg/summoner/userName=";
				string opggLink;
				string opgg = "";
				// Redacción de la lista
				foreach (var player in list) {
					UniOviUser uou = player as UniOviUser;
					usuario += $"{uou.Username}\n";
					invocador += $"{uou.LoLUsername}\n";
					opggLink = opggLinkBase + uou.LoLUsername.Replace(" ","%20");
					opgg += $"[Link]({opggLink})\n";	}
				// Creación y envío de la lista
				EmbedBuilder builder = new EmbedBuilder();
				builder.AddField("Usuario", usuario, true);
				builder.AddField("Nombre de invocador", invocador, true);
				builder.AddField("OP.gg",opgg, true);
				await Context.Channel.SendMessageAsync(embed: builder.Build());
			}

			/// <summary>
			/// Añade un nuevo usuario y su nombre de invocador a la base de datos
			/// </summary>
			/// <param name="username">Nombre de invocador del usuario</param>
			/// <returns></returns>
			[Command("add"), Alias("new"), Summary("Vincula tu nombre de invocador")]
			public async Task Add([Remainder] string username = null) {
				// Comprueba que se haya introducido un nombre de invocador válido, si lo ha hecho avisa y cierra
				if (username == null) {
					await Context.Channel.SendMessageAsync(":x: Debes introducir un nombre de invocador válido, reintroduce el comando correctamente: +lol add **<nombre de invocador>**");
					return;		}
				// Comprueba si el usuario tiene ya entrada en la base de datos, si no lo tiene lo añade
				if (!DataHandler.CheckUser(Context.User.Id)) {
					DataHandler.Add(Context.User.Id, Context.User.Username, username);
					await Context.Channel.SendMessageAsync(":ok_hand: Has sido añadido correctamente a la lista");
					return;		}
				// Comprueba si el nombre de invocador de su entrada está vacío, si no lo está avisa y cierra
				if (DataHandler.GetLolUsername(Context.User.Id) != "")
					await Context.Channel.SendMessageAsync(":x: Ya hay un nombre de invocador vinculado a tu usuario, si quieres modificarlo usa el siguiente comando: +lol **mod** <nombre de invocador>");
				else {
					// Establece el nombre de invocador
					DataHandler.SetLolUsername(Context.User.Id, username);
					await Context.Channel.SendMessageAsync(":ok_hand: Has sido añadido correctamente a la lista");	}
				return;
			}

			/// <summary>
			/// Modifica el nombre de invocador asociado a un usuario
			/// </summary>
			/// <param name="username">Nuevo nombre de invocador</param>
			/// <returns></returns>
			[Command("mod"), Alias("change"), Summary("Modifica tu nombre de invocador")]
			public async Task Mod([Remainder] string username = null) {
				// Comprueba que se haya introducido un nombre de invocador válido, si lo ha hecho avisa y cierra
				if (username == null) {
					await Context.Channel.SendMessageAsync(":x: Debes introducir un nombre de invocador válido, reintroduce el comando correctamente: +lol mod **<nombre de invocador>**");
					return;
				}
				// Comprueba si el usuario tiene ya entrada en la base de datos, si no lo tiene avisa y cierra
				if (!DataHandler.CheckUser(Context.User.Id)) {
					await Context.Channel.SendMessageAsync(":x: No te has añadido a la lista previamente, para hacerlo emplea el siguiente comando: +lol **add** <nombre de invocador>");
					return;
				}
				DataHandler.SetLolUsername(Context.User.Id, username);
				await Context.Channel.SendMessageAsync(":ok_hand: Tu nombre de invocador ha sido modificado correctamente");
				return;
			}

			/// <summary>
			/// Elimina de la base de datos el nombre de invocador vinculado a un usuario
			/// </summary>
			/// <returns></returns>
			[Command("del"), Alias("erase"), Summary("Elimina tu nombre de invocador")]
			public async Task Del() {
				// Comprueba si el usuario tiene ya entrada en la base, si no lo tiene avisa y cierra
				if (!DataHandler.CheckUser(Context.User.Id)) {
					await Context.Channel.SendMessageAsync(":x: No te has añadido a la lista previamente por lo que no se te puede eliminar de ella");
					return;
				}
				// Comprueba si el usuario tenía ya el nombre de invoador vacío previamente
				if (DataHandler.GetLolUsername(Context.User.Id) == "")
					await Context.Channel.SendMessageAsync(":x: No tenías ningún nombre de invocador previamente asignado");
				else {
					DataHandler.SetLolUsername(Context.User.Id, "");
					await Context.Channel.SendMessageAsync(":ok_hand: Tu nombre de invocador ha sido eliminado correctamente");
				}
				return;
			}
		}
	}
}
