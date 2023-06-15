using System.Text.Json.Serialization;

namespace Library.Domain.Models
{
	public class Book
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public DateTime CreationDate { get; set; }

		public string? Image { get; set; }

		[JsonIgnore]
		public Guid UserId { get; set; }

		public User User { get; set; }

		[JsonIgnore]
		public BookText BookText { get; set; }
	}
}
