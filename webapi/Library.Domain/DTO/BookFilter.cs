namespace Library.Domain.DTO
{
	public class BookFilter
	{
		public string? Name { get; set; }

		public int? CurrentPage { get; set; }

		public int? PageSize { get; set; }
	}
}
