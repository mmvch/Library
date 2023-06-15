using System.Text.Json.Serialization;

namespace Library.Domain.Models
{
	public class User
	{
		public Guid Id { get; set; }

		public string Login { get; set; }

		[JsonIgnore]
		public byte[] PasswordHash { get; set; }

		[JsonIgnore]
		public byte[] PasswordSalt { get; set; }

		[JsonIgnore]
		public List<Role> Roles { get; set; }

		[JsonIgnore]
		public List<Book> Books { get; set; }
	}
}
