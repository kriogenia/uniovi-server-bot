using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using UniOviServerBot.Resources.Datatypes;

namespace UniOviServerBot.Core.Commands
{
	public class MinecraftCommand : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Grupo de comandos minecraft
		/// </summary>
		[Group("minecraft"), Alias("mc"), Summary("Gestiona la información del servidor de Minecraft en caso de haberlo")]
		public class MinecraftGroup : ModuleBase<SocketCommandContext>
		{
			private Minecraft mc = new Minecraft();

			/// <summary>
			/// Muestra la información del servidor actual
			/// </summary>
			/// <returns></returns>
			[Command(""), Alias("show", "info"), Summary("Muestra la información del servidor actual")]
			public async Task List() {
				await Context.Channel.SendMessageAsync(embed: Show().Build());
			}

			/// <summary>
			/// Modifica la IP del servidor actual
			/// </summary>
			/// <returns></returns>
			[Command("changeip"), Alias("ip"), Summary("Modifica la dirección IP del servidor")]
			public async Task ChangeIp(string newIP = null) {
				// Comprueba que se haya introducido una IP, en caso contrario avisa y cierra
				if (newIP == null) {
					await Context.Channel.SendMessageAsync("No has introducido ninguna IP, prueba de nuevo con el comando: +minecraft changeIP **<nueva IP del servidor>**");
					return;
				}
				// Modifica la IP, la guarda y confirma el cambio
				mc.ChangeIp(newIP);
				await Context.Channel.SendMessageAsync($":ok_hand: La IP del servidor se ha modificado correctamente a {newIP}");
			}

			/// <summary>
			/// Modifica el tipo de servidor
			/// </summary>
			/// <param name="newType">Nuevo tipo de servidor</param>
			/// <returns></returns>
			[Command("changetype"), Alias("type"), Summary("Modifica el tipo del servidor")]
			public async Task ChangeType(string newType = null) {
				// Comprueba que se haya introducido una cadena, en caso contrario avisa y cierra
				if (newType == null) {
					await Context.Channel.SendMessageAsync("No has introducido ninguna cadena para especificar el tipo, prueba de nuevo con el comando: +minecraft changeType **<nuevo tipo de servidor>**");
					return;
				}
				// Modifica la IP, la guarda y confirma el cambio
				mc.ChangeType(newType);
				await Context.Channel.SendMessageAsync($":ok_hand: El tipo de servidor se ha modificado correctamente a {newType}");
			}

			/// <summary>
			/// Cambia el estado del servidor a offline
			/// </summary>
			/// <returns></returns>
			[Command("offline"), Alias("off"), Summary("Establece el servidor como offline")]
			public async Task SetOffline() {
				mc.SetOffline();
				await Context.Channel.SendMessageAsync($":ok_hand: El servidor se ha establecido offline correctamente :sleeping:");
			}

			/// <summary>
			/// Cambia el estado del servidor a en línea
			/// </summary>
			/// <returns></returns>
			[Command("online"), Alias("on"), Summary("Establece el servidor como online")]
			public async Task SetOnline() {
				mc.SetOnline();
				EmbedBuilder builder = Show();
				builder.WithAuthor($"El servidor se ha establecido online correctamente");
				await Context.Channel.SendMessageAsync(embed: builder.Build());
			}

			public EmbedBuilder Show() {
				EmbedBuilder builder = new EmbedBuilder();
				// Establece la descripción en base a que el servidor esté o no online
				if (mc.IsOnline)
					builder.WithDescription(":white_check_mark: **Online**");
				else
					builder.WithDescription(":red_circle: Offline");
				// Añade el resto de información y la devuelve
				builder.AddField("IP", mc.IP, true);
				builder.AddField("Tipo", mc.Type, true);
				builder.WithThumbnailUrl("https://static.planetminecraft.com/files/avatar/1584923_0.png");
				return builder;
			}
		}
	}
}
