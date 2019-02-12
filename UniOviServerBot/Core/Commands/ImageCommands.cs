using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Tajes.Core.Commands
{
	public class Comandos_de_imágenes : ModuleBase<SocketCommandContext> 
	{
		/// <summary>
		/// Muestra el avatar del usuario seleccionado
		/// </summary>
		/// <param name="user">Usuario del que mostrar el avatar</param>
		/// <returns></returns>
		[Command("avatar"), Summary("Muestra el avatar del usuario seleccionado")]
		public async Task avatar(IUser user = null) {
			// Comprueba si se ha pasado un usuario válido
			if (user == null) {
				await Context.Channel.SendMessageAsync(":x: Debes especificar un usuario del que mostrar el avatar, repite el comando de la forma: +avatar **<userID>**");
				return;
			}
			// Genera el mensaje con el avatar del usuario solicitado
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor("Avatar de " + user.Username);
			embed.WithColor(255, 150, 50);
			embed.WithImageUrl(user.GetAvatarUrl());
			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		/// <summary>
		/// Responde con una imagen de Ayer Sir!
		/// </summary>
		/// <returns></returns>
		[Command("ayesir"), Summary("Responde con un Aye Sir!")]
		public async Task AyeSir() {
			// Genera el mensaje
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor("Aye Sir!");
			embed.WithColor(50, 150, 250);
			embed.WithImageUrl("https://i.imgur.com/IUvfCyi.png");
			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		/// <summary>
		/// Muestra una F y un mensaje en pantalla para mostrar los respetos
		/// </summary>
		/// <returns></returns>
		[Command("f"), Summary("Press F to pay respects")]
		public async Task F(IUser user = null) {
			// Establece el objetivo del comando
			string to;
			if (user == null) to = "";
			else to = " to " + user.Username;
			// Cosntruye y envía el mensaje
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor(Context.User.Username + " paid their respects" + to);
			embed.WithColor(255, 150, 50);
			embed.WithImageUrl("http://www.drodd.com/images15/orange-f1.png");
			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		/// <summary>
		/// Muestra el sticker de Mario comiendo macarrones
		/// </summary>
		/// <returns></returns>
		[Command("mariarroni"), Summary("SOMEBODY TOUCH MY SPAGHETTI - Mario version")]
		public async Task Mariarroni() {
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithImageUrl("https://i.imgur.com/CTKs3cG.png");
			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}
	}
}
