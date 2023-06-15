namespace Library.Domain.Models
{
	public class BookText
	{
		public Guid Id { get; set; }

		public string? Text { get; set; }

		public Book Book { get; set; }
	}
}
