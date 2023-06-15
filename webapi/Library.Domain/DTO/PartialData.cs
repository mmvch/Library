namespace Library.Domain.DTO
{
	public class PartialData<TEntity> where TEntity : class
	{
		public IEnumerable<TEntity> Data { get; set; }
		public int TotalAmount { get; set; }
	}
}
