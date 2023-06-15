using Microsoft.AspNetCore.Http;

namespace Library.DAL.Services
{
	public interface ITextService
	{
		Task<string> ReadAsync(IFormFile formFile);
		Task<IEnumerable<IEnumerable<string>>?> GetAsync(Guid id);
	}
}
