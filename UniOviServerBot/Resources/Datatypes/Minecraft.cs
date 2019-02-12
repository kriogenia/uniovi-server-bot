using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace UniOviServerBot.Resources.Datatypes
{
	/// <summary>
	/// Gestiona la información del servidor de Minecraft
	/// </summary>
	public class Minecraft
	{
		// Propiedades
		public bool IsOnline { get; private set; }
		public string IP { get; private set; }
		public string Type { get; private set; }
		// Ruta del fichero
		public static string PATH = "Data/minecraft.txt";

		/// <summary>
		/// Constructor de la clase, carga la información del fichero
		/// </summary>
		public Minecraft() {
			LoadInfo();
		}

		/// <summary>
		/// Lee el fichero de información del servidor y carga la información al objeto
		/// </summary>
		private void LoadInfo() {
			string[] lines = File.ReadAllLines(PATH, Encoding.UTF8);
			IsOnline = Int32.Parse(lines[0]) == 1;
			IP = lines[1];
			Type = lines[2];
		}

		/// <summary>
		/// Guarda la información del servidor en el archivo
		/// </summary>
		private void SaveInfo() {
			string online;
			if (IsOnline)
				online = "1";
			else
				online = "0";
			string[] info = { online, IP, Type };
			File.WriteAllLines(PATH, info);
		}

		/// <summary>
		/// Establece el servidor como offline
		/// </summary>
		public void SetOffline() {
			IsOnline = false;
			SaveInfo();
		}

		/// <summary>
		/// Establece el servidor como online
		/// </summary>
		public void SetOnline() {
			IsOnline = true;
			SaveInfo();
		}

		/// <summary>
		/// Modifica la IP del servidor
		/// </summary>
		/// <param name="newIp">Nueva IP del servidor</param>
		public void ChangeIp(string newIp) {
			IP = newIp;
			SaveInfo();
		}

		/// <summary>
		/// Modifica el tipo de servidor
		/// </summary>
		/// <param name="newType">Nuevo tipo del servidor</param>
		public void ChangeType(string newType) {
			Type = newType;
			SaveInfo();
		}


	}
}
