using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;

namespace Tajes
{
	public class Program
	{
		private CommandService commands;												// Gestor de comandos
		private DiscordSocketClient client;												// Cliente

		/// <summary>
		/// Arranca el bot, de forma asíncrona para la conexión con Discord
		/// </summary>
		public static void Main(string[] args) {
			new Program().MainAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		/// Crea la conexión asíncrona
		/// </summary>
		/// <returns></returns>
		public async Task MainAsync() {
			// Creación del cliente, con información de log de debug
			client = new DiscordSocketClient(new DiscordSocketConfig {
				LogLevel = LogSeverity.Debug
			});
			// Creación del servicio de comandos
			commands = new CommandService(new CommandServiceConfig {
				CaseSensitiveCommands = true,
				DefaultRunMode = RunMode.Async,
				LogLevel = LogSeverity.Debug
			});
			// Lectura de mensajes y comandos
			client.MessageReceived += MessageReceived;
			await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);
			// Gestión del log y del jugando
			client.Ready += Ready;
			client.Log += Log;
			//  Lectura del token del bot y logueo
			string token = File.ReadAllText("Data/token.txt");
			await client.LoginAsync(TokenType.Bot, token);
			await client.StartAsync();

			// Bloquea esta tarea hasta que el programa se cierra
			await Task.Delay(-1);
		}

		/// <summary>
		/// Gestiona los mensajes que el bot lee
		/// </summary>
		/// <param name="message">Mensaje leído</param>
		/// <returns>Tarea asíncrona realizada</returns>
		private async Task MessageReceived(SocketMessage paramMessage) {
			var message = paramMessage as SocketUserMessage;
			var context = new SocketCommandContext(client, message);
			// Comprobación del comando
			if (message == null || context.Message.Content == "")
				return;
			if (context.User.IsBot)
				return;
			int argPos = 0;
			if (!(message.HasCharPrefix('+', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
				return;
			// Ejecucicón del comando
			var result = await commands.ExecuteAsync(context, argPos, null);
			// Debug en caso de ejecución errónea
			if (!result.IsSuccess)
				Console.WriteLine($"{DateTime.Now} at Commands | {context.Message.Content} | Error: {result.ErrorReason}");
		}

		/// <summary>
		/// Establece el juego de status del bot
		/// </summary>
		private async Task Ready() {
			await client.SetGameAsync("+comando");
		}
		/// <summary>
		/// Gestiona la tarea de log
		/// </summary>
		/// <param name="log">Mensaje de log</param>

		private static Task Log(LogMessage log) {
			var cc = Console.ForegroundColor;
			switch (log.Severity) {
				case LogSeverity.Critical:
				case LogSeverity.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogSeverity.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogSeverity.Info:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case LogSeverity.Verbose:
				case LogSeverity.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
			}
			Console.WriteLine($"{DateTime.Now,-19} [{log.Severity,8}] {log.Source}: {log.Message}");
			Console.ForegroundColor = cc;
			
			return Task.CompletedTask;
		}
	}
}