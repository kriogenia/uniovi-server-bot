using System.ComponentModel.DataAnnotations;

namespace Tajes.Resources.Database
{
	public class UniOviUser
	{
		[Key]	// Clave de la entidad
		public ulong UserID { get; set; }
		// Resto de campos
		public string Username { get; set; }
		public string LoLUsername { get; set; }
	}
}
