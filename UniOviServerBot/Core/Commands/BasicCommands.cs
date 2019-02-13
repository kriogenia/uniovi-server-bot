using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Tajes.Core.Commands
{
	/// <summary>
	/// Contiene comandos básicos simples
	/// </summary>
	public class Comandos_Básicos : ModuleBase<SocketCommandContext>
	{
		public const string BASESAY = "Pues si no me especificas que decir solo puedo decir: **Espárrago**";
		//public const string BASESAY = "?ping";

		/// <summary>
		/// Dice lo especificado en el mensaje, útil para proarlo en interacciones con otros bots
		/// </summary>
		/// <returns></returns>
		[Command("say"), Summary("El bot repite lo que le has mandado")]
		public async Task Say([Remainder] string input = BASESAY) {
			await Context.Channel.DeleteMessageAsync(Context.Message.Id);
			await Context.Channel.SendMessageAsync(input);
		}

		/// <summary>
		/// Muestra una lista de enlaces a las asignaturas en el campus virtual
		/// </summary>
		/// <returns></returns>
		[Command("campus"), Summary("Muestra una lista con enlaces a las asignaturas en el campus virtual")]
		public async Task Campus() {
			// Inicializa los parámetros necesarios
			// string path = "Data/campus.txt";
			string path = Environment.CurrentDirectory + "/../../../Data/campus.txt";
			string[] lines = File.ReadAllLines(path);
			string[] fields;
			string desc = "";
			EmbedBuilder builder = new EmbedBuilder();
			builder.WithAuthor("Campus Virtual");
			builder.WithThumbnailUrl("http://www.epigijon.uniovi.es/images/logo/Uniovi_central_redu_web_convi.png");
			// Recorre las líneas leídas y las añade al texto de la descripción
			foreach (var line in lines) {
				fields = line.Split("@");
				desc += fields[0]+ " - ";
				desc += $"**[{fields[1]}]({fields[2]})**\n";
			}
			builder.WithDescription(desc);
			await Context.Channel.SendMessageAsync(embed: builder.Build());
		}

		/// <summary>
		/// ñe
		/// </summary>
		/// <returns></returns>
		[Command("ñe"), Summary("ñeñeñeñeñeñeñeñeñeñeñeñe")]
		public async Task Ñe() {
			string ñe = "ñeñeñeñeñeñeñeñeñeñeñeñe";
			string ñeñe = "";
			int number = new Random().Next(1, 10);
			for (int i = 0; i < number; i++)
				ñeñe += ñe;
			await Context.Channel.SendMessageAsync(ñeñe);
		}
	}
}
