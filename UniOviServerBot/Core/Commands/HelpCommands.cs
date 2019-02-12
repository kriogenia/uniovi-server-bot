using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace UniOviServerBot.Core.Commands
{
	public class Ayuda : ModuleBase
	{
		private readonly CommandService _commands;

		public Ayuda(CommandService service) {
			_commands = service;
		}

		/// <summary>
		/// Lista todos los comandos u ofrece la ayuda completa
		/// </summary>
		/// <param name="commandOrModule">Nombre del comando o módulo concreto a inspeccionar</param>
		/// <returns></returns>
		[Command("help"), Summary("Lista todos los comandos")]
		public async Task HelpAsync([Remainder] string commandOrModule = null) {
			// Si ha especficiado comando o módulo, ofrece la información detallada del mismo
			if (commandOrModule != null) {
				await DetailedHelpAsync(commandOrModule.ToLower());
				return;
			}
			// Si no lo ha hecho lista, todos los comandos
			var builder = new EmbedBuilder();
			{
				builder.WithColor(new Color(87, 222, 127));
				builder.WithTitle($"Ains, {Context.User.Username}, aquí tienes la lista completa de comandos.");
			}
			// Recorremos los módulos
			foreach (var module in _commands.Modules) {
				string fieldValue = null;
				// Recorrremos los comandos del módulo
				foreach (var cmd in module.Commands) {
					// Reporte en caso de ausencia de Summary
					if (string.IsNullOrWhiteSpace(cmd.Summary))
						Console.WriteLine("No summary for " + cmd.Name);
					var result = await cmd.CheckPreconditionsAsync(Context);
					if (result.IsSuccess)
						fieldValue += $"`{cmd.Aliases.First().Replace(module.Name+" ","")}`, ";
				}
				// Salta al siguiente módulo si no se ha rellenado
				if (string.IsNullOrWhiteSpace(fieldValue))
					continue;
				// Hay información así que creamos el embed
				fieldValue = fieldValue.Substring(0, fieldValue.Length - 2);
				builder.AddField(
					x => {
						x.Name = $"\n · {module.Name.Replace("_"," ")}";
						x.Value = $"{fieldValue}";
						x.IsInline = false;
					});
			}
			// Devuelve la ayuda generada
			await ReplyAsync(embed: builder.Build());
		}

		/// <summary>
		/// Ofrece mayor ayuda con un comando concreto
		/// </summary>
		/// <param name="asked"> El comando </param>
		/// <returns></returns>
		private async Task DetailedHelpAsync([Remainder] string asked) {
			var moduleFound = _commands.Modules.Select(mod => mod.Name.ToLower()).ToList().Contains(asked);
			// Comprobamos si lo pedido es un módulo
			if (moduleFound) {
				await DetailedModuleHelpAsync(asked);
				return;
			}
			// Comprobamos si es un comando
			var result = _commands.Search(Context, asked);
			if (!result.IsSuccess) {
				await ReplyAsync(
					$"No hay ningún comando llamado *{asked}*");
				return;
			}
			// Creación del builder
			var builder = new EmbedBuilder {
				Color = new Color(87, 222, 127)
			};
			// Elaboración del builder con los comandos pedidos
			foreach (var cmd in result.Commands.Select(match => match.Command))
				builder.AddField(
					x => {
						x.Name = $"Ayuda de *{asked}* en camino";
						var temp = "No hay";
						// Si el comando tiene más de un alias lsita todos
						if (cmd.Aliases.Count != 1) temp = string.Join(", ", cmd.Aliases);
						x.Value = "**Alias**: " + temp;
						//temp = "`" + Config.Bot.PrefixDictionary[Context.Guild.Id] + asked;
						// Completa la línea con la información de los parámetros
						if (cmd.Parameters.Count != 0)
							temp += " " + string.Join(
										" ",
										cmd.Parameters.Select(
											p => p.IsOptional
												? "<" + (p.Summary.Length > 1 ? p.Summary : p.Name) + ">"
												: "[" + (p.Summary.Length > 1 ? p.Summary : p.Name) + "]"));

						temp += "`";
						x.Value += $"\n**Uso**: {temp}\n**Resumen**: {cmd.Summary}";
						x.IsInline = false;
					});
			// Aclaración de uso
			builder.WithFooter("Nota: Parámetros entre []  son obligatorios, mientras que los que están entre <> son opcionales");
			// Imprime la ayuda
			await ReplyAsync(embed: builder.Build());
		}

		/// <summary>
		/// La ayuda detallada de los módulos
		/// </summary>
		/// <param name="module"> El modulo </param>
		/// <returns> The <see cref="Task" /> </returns>
		private async Task DetailedModuleHelpAsync(string module) {
			var first = _commands.Modules.First(mod => mod.Name.ToLower() == module);
			var embed = new EmbedBuilder {
				Title = "Lista de comandos de " + module.ToUpper(),
				Description = string.Empty,
				Color = new Color(87, 222, 127)
			};
			embed.WithFooter("Usa 'help <nombre del comando>' para obtener más información del comando");
			foreach (var cmds in first.Commands)
					embed.Description += $"{cmds.Name}, ";
			embed.Description = embed.Description.Substring(1, embed.Description.Length - 2);
			await ReplyAsync(string.Empty, false, embed.Build());
		}
	}
}
