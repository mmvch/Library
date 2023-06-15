using Microsoft.AspNetCore.Http;

namespace Library.DAL.Services
{
	public interface IImageService
	{
		Task<string> ReplaceAsync(IFormFile formFile, string baseName);
		Task<string> UploadAsync(IFormFile formFile);
		Task DeleteAsync(string baseName);
	}
}
